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
            this.itemRepository.FindAsync(id).GetAwaiter().GetResult() ?? throw new Exception();

        public Task<IEnumerable<Item>> GetAllItems() =>
            this.itemRepository.GetAll(includeProperties: nameof(OnlineShop));

        public bool IsItemExisting(int id) =>
            this.itemRepository.IsExisting(id).ConfigureAwait(false).GetAwaiter().GetResult();

        private bool IsItemExisting(string url) =>
            this.itemRepository.GetAll(filter: item => item.URL == url).GetAwaiter().GetResult().FirstOrDefault() != null;

        public async Task CreateItem(string itemURL, OnlineShop onlineShop, ItemType type)
        {
            try
            {
                if (!this.IsItemExisting(itemURL))
                {
                    var loadedItem = await ItemService.LoadItemFromWeb(itemURL, onlineShop, type).ConfigureAwait(false);
                    var item = await this.itemRepository.Add(loadedItem).ConfigureAwait(false);
                    onlineShop.AddItem(item);

                    await this.itemPriceService.CreateItemPrice(item.CurrentPrice, item.Id).ConfigureAwait(false);

                    logger.Debug($"Saved item: '{item}' to database.");
                    EventsLocator.ItemAdded.Publish(item);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Couldn't create an item.");
                throw;
            }
        }

        public void UpdateItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.itemRepository.Update(item).ConfigureAwait(false);
                    logger.Debug($"Updated item: '{item}'.");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Couldn't update an item.");
                throw;
            }
        }

        public async Task<bool> DeleteItem(Item item)
        {
            try
            {
                bool itemDeleted = false;
                if (this.IsItemExisting(item.Id))
                {
                    itemDeleted = await this.itemRepository.Delete(item).ConfigureAwait(false);
                    item.OnlineShop.DeleteItem(item);

                    logger.Debug($"Deleted item: '{item}'.");

                    EventsLocator.ItemDeleted.Publish(item);
                }

                return itemDeleted;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't delete item: '{item}'.");
                throw;
            }
        }

        public async Task<ItemPrice> UpdateItemPrice(Item item)
        {
            try
            {
                ItemPrice newItemPrice = null;
                if (this.IsItemExisting(item.Id))
                {
                    var updatedItem = await ItemService.LoadItemFromWeb(item.URL, item.OnlineShop, item.Type).ConfigureAwait(false);

                    if (updatedItem != item)
                    {
                        item.Description = updatedItem.Description;
                        item.CurrentPrice = updatedItem.CurrentPrice;

                        await this.itemRepository.Update(item).ConfigureAwait(false);

                        item.OnlineShop.UpdateItem(updatedItem);

                        newItemPrice = await this.itemPriceService.CreateItemPrice(updatedItem.CurrentPrice, updatedItem.Id).ConfigureAwait(false);
                    }
                }

                return newItemPrice;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't update item price for '{item}'.");
                throw;
            }
        }

        private static Task<Item> LoadItemFromWeb(string itemUrl, OnlineShop onlineShop, ItemType type)
        {
            var htmlService = new HtmlWeb();
            var itemDocument = htmlService.Load(itemUrl);

            return RetrieveItemData.CreateItem(itemUrl, itemDocument, onlineShop, type);
        }
    }
}
