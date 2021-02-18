using System;
using System.Collections.Generic;
using System.Linq;

using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemPriceService : IItemPriceService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemPriceService));

        private readonly IRepository<ItemPrice> itemPriceRepository;
        private readonly IRepository<Item> itemRepository;

        public ItemPriceService(IRepository<ItemPrice> itemPriceRepository, IRepository<Item> itemRepository)
        {
            this.itemPriceRepository = itemPriceRepository;
            this.itemRepository = itemRepository;
        }

        public IEnumerable<ItemPrice> GetPricesForItem(int itemId) =>
           this.itemPriceRepository.GetAll(filter: i => i.ItemId == itemId).Result;

        public void CreateItemPrice(ItemPrice itemPrice)
        {
            try
            {
                if (this.IsItemExisting(itemPrice.ItemId))
                {
                    this.itemPriceRepository.Add(itemPrice);
                    logger.Debug($"Saved '{itemPrice}' for itemId: {itemPrice.ItemId} in database");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error when saving item price for itemId: {itemPrice.ItemId}.\t{e}");
                throw;
            }
        }

        public double GetLatestItemPrice(int itemId)
        {
            try
            {
                return this.itemPriceRepository.GetAll(
                    filter: price => price.ItemId == itemId,
                    orderBy: prices => prices.OrderByDescending(price => price.PriceDate))
                .Result.First()
                .Price;
            }
            catch (Exception e)
            {
                logger.Error($"Error when retrieving item price for itemId: {itemId}.\t{e}");
                return 0;
            }
        }

        private bool IsItemExisting(int id) =>
           this.itemRepository.IsExisting(id).Result;
    }
}
