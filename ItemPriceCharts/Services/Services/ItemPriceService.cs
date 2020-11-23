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

        private readonly IModelsContext modelsContext;

        public ItemPriceService(ModelsContext modelsContext)
        {
            this.modelsContext = modelsContext;
        }

        public IEnumerable<ItemPrice> GetPricesForItem(int itemId) =>
           this.modelsContext.ItemPriceRepository.All(filter: i => i.ItemId == itemId).Result;

        public void CreateItemPrice(ItemPrice itemPrice)
        {
            try
            {
                if (this.IsItemExisting(itemPrice.ItemId))
                {
                    this.modelsContext.ItemPriceRepository.Add(itemPrice);
                    this.modelsContext.CommitChangesAsync();
                    logger.Debug($"Saved '{itemPrice}' for itemId: {itemPrice.ItemId} in database");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error when saving item price for itemId: {itemPrice.ItemId}.\t{e}");
                throw e;
            }
        }

        public double GetLatestItemPrice(int itemId)
        {
            try
            {
                return this.modelsContext.ItemPriceRepository.All(
                    filter: price => price.ItemId == itemId,
                    orderBy: prices => prices.OrderByDescending(price => price.PriceDate))
                .Result.First()
                .Price;
            }
            catch (Exception e)
            {
                logger.Error($"Error when retrieving item price for itemId: {itemId}.\t{e}");
                throw e;
            }
        }

        private bool IsItemExisting(int id) =>
           this.modelsContext.ItemRepository.IsExisting(id).Result;
    }
}
