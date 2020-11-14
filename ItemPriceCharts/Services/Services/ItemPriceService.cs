using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemPriceService : IItemPriceService
    {
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
                }
            }
            catch (Exception e)
            {
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
                throw e;
            }
        }

        private bool IsItemExisting(int id) =>
           this.modelsContext.ItemRepository.IsExisting(id).Result;
    }
}
