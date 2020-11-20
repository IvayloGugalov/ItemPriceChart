using System;
using System.Collections.Generic;
using System.Linq;

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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IModelsContext modelsContext;
        private readonly IItemPriceService itemPriceService;

        public ItemService(ModelsContext modelsContext, ItemPriceService itemPriceService)
        {
            this.modelsContext = modelsContext;
            this.itemPriceService = itemPriceService;
        }

        public Item FindItem(int id) =>
            this.modelsContext.ItemRepository.FindAsync(id).Result ?? throw new Exception();

        public IEnumerable<Item> GetAllItems() => this.modelsContext.ItemRepository.All().Result;

        public IEnumerable<Item> GetItemsForShop(OnlineShop onlineShop) =>
            this.modelsContext.ItemRepository.All(filter: item => item.OnlineShop.Id == onlineShop.Id).Result;

        public bool IsItemExisting(int id) =>
            this.modelsContext.ItemRepository.IsExisting(id).Result;

        internal bool IsItemExisting(string url) =>
            this.modelsContext.ItemRepository.All(filter: item => item.URL == url).Result.FirstOrDefault() != null;

        public void CreateItem(string itemURL, OnlineShop onlineShop, ItemType type)
        {
            try
            {
                if (!this.IsItemExisting(itemURL))
                {
                    var item = this.LoadItemFromWeb(itemURL, onlineShop, type);

                    this.modelsContext.ItemRepository.Add(item);
                    this.modelsContext.CommitChangesAsync();
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
                logger.Debug($"Couldn't create an item: {e}");
                throw e;
            }
        }

        public void UpdateItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.modelsContext.ItemRepository.Update(item);
                    this.modelsContext.CommitChangesAsync();
                    logger.Debug($"Updated item: '{item}'");
                }
            }
            catch (Exception e)
            {
                logger.Debug($"Couldn't update an item: {e}");
                throw e;
            }
        }

        public void DeleteItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.modelsContext.ItemRepository.Delete(item);
                    this.modelsContext.CommitChangesAsync();
                    logger.Debug($"Deleted item: '{item}'");

                    EventsLocator.ItemDeleted.Publish(item);
                }
            }
            catch (Exception e)
            {
                logger.Debug($"Couldn't delete item: '{item}'.\t{e}");
                throw e;
            }
        }

        public ItemPrice UpdateItemPrice(Item item)
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

                        this.modelsContext.ItemRepository.Update(item);
                        logger.Debug($"Updated item: {item.Title}:" +
                            $"\nFrom {item.CurrentPrice} to {updatedItem.Description}" +
                            $"\nFrom {item.CurrentPrice} to {updatedItem.CurrentPrice}");

                        return this.CreateNewItemPrice(item.CurrentPrice, item.Id);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                logger.Debug($"Couldn't update item price for '{item}'.\t{e}");
                throw e;
            }
        }

        private Item LoadItemFromWeb(string itemUrl, OnlineShop onlineShop, ItemType type)
        {
            var htmlService = new HtmlWeb();
            var itemDocument = htmlService.Load(itemUrl);

            return RetrieveItemData.CreateItem(itemUrl, itemDocument, onlineShop, type);
        }

        private ItemPrice CreateNewItemPrice(double price, int itemId)
        {
            var newItemPrice = new ItemPrice(
                                   currentPrice: price,
                                   itemId: itemId);
            this.itemPriceService.CreateItemPrice(newItemPrice);

            return newItemPrice;
        }
    }
}
