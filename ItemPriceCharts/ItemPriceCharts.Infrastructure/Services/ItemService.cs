﻿using System;
using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemService));

        public async Task AddItemToShop(string itemURL, OnlineShop onlineShop, ItemType type)
        {
            try
            {
                using ModelsContext dbContext = new();

                var itemAlreadyExists = dbContext.Items.Where(item => item.URL == itemURL).SingleOrDefault() != null;

                if (!itemAlreadyExists)
                {
                    var (title, description, price) = await ItemService.LoadItemFromWeb(itemURL, onlineShop.Title).ConfigureAwait(false);

                    dbContext.BeginTransaction();

                    onlineShop.AddItem(new Item(
                                           url: itemURL,
                                           title: title,
                                           description: description,
                                           price: new ItemPrice(price),
                                           onlineShop: onlineShop,
                                           type: type),
                                       dbContext);

                    dbContext.CommitToDatabase();
                    logger.Debug($"Saved item: '{title}' to database.");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Couldn't create an item.");
                throw;
            }
        }

        public async Task UpdateItem(Item item)
        {
            try
            {
                using ModelsContext dbContext = new();

                var isItemExisting = await dbContext.Items.FindAsync(item.Id) != null;

                if (isItemExisting)
                {
                    dbContext.BeginTransaction();

                    dbContext.Update(item);

                    dbContext.CommitToDatabase();
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
                using ModelsContext dbContext = new();

                var isItemExisting = await dbContext.Items.FindAsync(item.Id) != null;

                if (isItemExisting)
                {
                    dbContext.Remove(item);
                    item.OnlineShop.RemoveItem(item);

                    logger.Debug($"Deleted item: '{item}'.");

                    return true;
                }

                return false;
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
                using ModelsContext dbContext = new();

                var isItemExisting = await dbContext.Items.FindAsync(item.Id) != null;
                if (isItemExisting)
                {
                    (_, _, double newPrice) = await ItemService.LoadItemFromWeb(item.URL, item.OnlineShop.Title).ConfigureAwait(false);

                    if (newPrice != item.CurrentPrice.Price)
                    {
                        var itemPrice = new ItemPrice(newPrice);
                        var updatedItem = item.UpdateItemPrice(itemPrice);

                        dbContext.BeginTransaction();

                        dbContext.Items.Update(item);

                        dbContext.CommitToDatabase();

                        logger.Debug($"Update the price of item {updatedItem}.");

                        return itemPrice;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't update item price for '{item}'.");
                throw;
            }
        }

        private static async Task<(string title, string description, double price)> LoadItemFromWeb(string itemUrl, string shopName)
        {
            var htmlService = new HtmlWeb();
            var itemDocument = htmlService.Load(itemUrl);

            return await Task.Run(() => ItemDataRetrieveService.CreateItem(itemDocument, shopName));
        }

    }
}
