using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;
using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Helpers;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemService : IItemService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemService));

        private readonly IItemPriceService itemPriceService;
        private readonly IRepository<Item> itemRepository;

        public ItemService(IItemPriceService itemPriceService, IRepository<Item> itemRepository)
        {
            this.itemPriceService = itemPriceService;
            this.itemRepository = itemRepository;
        }

        public Item FindItem(int id) =>
            this.itemRepository.FindAsync(id).Result ?? throw new Exception();

        public async Task<IEnumerable<Item>> GetAllItems() =>
            await this.itemRepository.GetAll(includeProperties: nameof(OnlineShop));

        public bool IsItemExisting(int id) =>
            this.itemRepository.IsExisting(id).Result;

        internal bool IsItemExisting(string url) =>
            this.itemRepository.GetAll(filter: item => item.URL == url).Result.FirstOrDefault() != null;

        public Task CreateItem(string itemURL, OnlineShop onlineShop, ItemType type)
        {
            try
            {
                if (!this.IsItemExisting(itemURL))
                {
                    var item = this.LoadItemFromWeb(itemURL, onlineShop, type);

                    this.itemRepository.Add(item);
                    onlineShop.AddItem(item);

                    logger.Debug($"Saved item: '{item}' to database");

                    this.itemPriceService.CreateItemPrice(new ItemPrice(
                        currentPrice: item.CurrentPrice,
                        itemId: item.Id));
                    logger.Debug($"Saved item price for itemId: {item.Id} to database");

                    EventsLocator.ItemAdded.Publish(item);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Couldn't create an item: {e}");
                throw;
            }

            return Task.CompletedTask;
        }

        public void UpdateItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.itemRepository.Update(item);
                    logger.Debug($"Updated item: '{item}'");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Couldn't update an item: {e}");
                throw;
            }
        }

        public Task<bool> DeleteItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.itemRepository.Delete(item);
                    item.OnlineShop.DeleteItem(item);

                    logger.Debug($"Deleted item: '{item}'");

                    EventsLocator.ItemDeleted.Publish(item);

                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception e)
            {
                logger.Error($"Couldn't delete item: '{item}'.\t{e}");
                throw;
            }
        }

        public async Task<ItemPrice> UpdateItemPrice(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    var updatedItem = this.LoadItemFromWeb(item.URL, item.OnlineShop, item.Type);

                    if (updatedItem != item)
                    {
                        item.Description = updatedItem.Description;
                        item.CurrentPrice = updatedItem.CurrentPrice;

                        await this.itemRepository.Update(item).ConfigureAwait(false);

                        logger.Debug($"Updated item: {item.Title}:" +
                            $"\nFrom {item.CurrentPrice} to {updatedItem.Description}" +
                            $"\nFrom {item.CurrentPrice} to {updatedItem.CurrentPrice}");

                        var newItemPrice = new ItemPrice(
                                   currentPrice: updatedItem.CurrentPrice,
                                   itemId: updatedItem.Id);
                        this.itemPriceService.CreateItemPrice(newItemPrice);

                        return newItemPrice;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                logger.Error($"Couldn't update item price for '{item}'.\t{e}");
                return null;
            }
        }

        private Item LoadItemFromWeb(string itemUrl, OnlineShop onlineShop, ItemType type)
        {
            var htmlService = new HtmlWeb();
            var itemDocument = htmlService.Load(itemUrl);

            return RetrieveItemData.CreateItem(itemUrl, itemDocument, onlineShop, type);
        }
    }
}
