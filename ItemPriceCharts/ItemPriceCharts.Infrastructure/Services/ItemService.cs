using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Data;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ItemService));
       
        private readonly ModelsContext dbContext;
        private readonly IHtmlWebWrapper htmlServiceWrapper;
        private readonly IItemDataRetrieveService itemDataRetrieveService;

        public ItemService(ModelsContext dbContext, IHtmlWebWrapper htmlServiceWrapper, IItemDataRetrieveService itemDataRetrieveService)
        {
            this.dbContext = dbContext;
            this.htmlServiceWrapper = htmlServiceWrapper;
            this.itemDataRetrieveService = itemDataRetrieveService;
        }

        public async Task AddItemToShop(string itemUrl, OnlineShop onlineShop, ItemType type)
        {
            try
            {
                var itemAlreadyExists = await this.dbContext.Items.SingleOrDefaultAsync(item => item.Url == itemUrl) != null;

                if (!itemAlreadyExists)
                {
                    var (title, description, price) = await this.LoadItemFromWeb(itemUrl, onlineShop.Title).ConfigureAwait(false);

                    var createdItem = new Item(
                        url: itemUrl,
                        title: title,
                        description: description,
                        price: price,
                        onlineShop: onlineShop,
                        type: type);

                    this.dbContext.OnlineShops.Attach(onlineShop);

                    // EF Core is tracking the Entity and will automatically add the Item to the DB here
                    onlineShop.AddItem(createdItem);

                    await this.dbContext.SaveChangesAsync();

                    Logger.Debug($"Saved a new item: '{title}' to database.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Couldn't create an item.");
                throw;
            }
        }

        public async Task UpdateItem(Item item)
        {
            try
            {
                var isItemExisting = await this.dbContext.Items.FindAsync(item.Id) != null;

                if (isItemExisting)
                {
                    this.dbContext.Update(item);

                    await this.dbContext.SaveChangesAsync();
                    Logger.Debug($"Updated item: '{item}'.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Couldn't update an item.");
                throw;
            }
        }

        public async Task<bool> DeleteItem(Item item)
        {
            try
            {
                var isItemExisting = await this.dbContext.Items.FindAsync(item.Id) != null;

                if (isItemExisting)
                {
                    var onlineShop = await dbContext.OnlineShops.FindAsync(item.OnlineShop.Id);

                    onlineShop.RemoveItem(item);
                    await this.dbContext.SaveChangesAsync();

                    Logger.Debug($"Deleted item: '{item.Title}'.");

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Couldn't delete item: '{item}'.");
                throw;
            }
        }

        public async Task<ItemPrice> TryUpdateItemPrice(Item item)
        {
            try
            {
                var itemInDb = await this.dbContext.Items.FindAsync(item.Id);

                if (itemInDb != null)
                {
                    var (_, _, newPrice) = await this.LoadItemFromWeb(item.Url, item.OnlineShop.Title).ConfigureAwait(false);

                    if (Math.Abs(newPrice - itemInDb.CurrentPrice.Price) >= 0.01)
                    {
                        var itemPrice = new ItemPrice(itemInDb.Id, newPrice);

                        itemInDb.UpdateItemPrice(itemPrice);

                        await this.dbContext.SaveChangesAsync();

                        Logger.Debug($"Update the price of item {item}.");

                        return itemPrice;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Couldn't update item price for '{item}'.");
                throw;
            }
        }

        private async Task<(string title, string description, double price)> LoadItemFromWeb(string itemUrl, string shopName)
        {
            var itemDocument = this.htmlServiceWrapper.LoadDocument(itemUrl);

            return await Task.Run(() => this.itemDataRetrieveService.CreateItem(itemDocument, shopName));
        }

    }
}
