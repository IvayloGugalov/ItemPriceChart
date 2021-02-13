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

        public IEnumerable<Item> GetAllItems() =>
            this.itemRepository.GetAll(includeProperties: nameof(OnlineShop)).Result;

        public IEnumerable<Item> GetItemsForShop(OnlineShop onlineShop) =>
            this.itemRepository.GetAll(filter: item => item.OnlineShop.Id == onlineShop.Id, includeProperties: nameof(OnlineShop)).Result;

        public bool IsItemExisting(int id) =>
            this.itemRepository.IsExisting(id).Result;

        internal bool IsItemExisting(string url) =>
            this.itemRepository.GetAll(filter: item => item.URL == url).Result.FirstOrDefault() != null;

        public void CreateItem(string itemURL, OnlineShop onlineShop, ItemType type)
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

        public void DeleteItem(Item item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    this.itemRepository.Delete(item);
                    item.OnlineShop.DeleteItem(item);

                    logger.Debug($"Deleted item: '{item}'");

                    EventsLocator.ItemDeleted.Publish(item);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Couldn't delete item: '{item}'.\t{e}");
                throw;
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

                        this.itemRepository.Update(item);
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
