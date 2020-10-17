using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemPriceService : IItemPriceService
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemPriceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateItemPrice(ItemPrice itemPrice)
        {
            try
            {
                if (this.IsItemExisting(itemPrice.ItemId))
                {
                    this.unitOfWork.ItemPriceRepository.Add(itemPrice);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ItemPrice> GetPricesForItem(int itemId) =>
           this.unitOfWork.ItemPriceRepository.All(filter: i => i.ItemId == itemId).Result;

        public double GetLatestItemPrice(int itemId)
        {
            try
            {
                return this.unitOfWork.ItemPriceRepository.All(
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

        public bool IsItemExisting(int itemId) =>
           this.unitOfWork.ItemRepository.All(i => i.Id == itemId).Result.First() != null;
    }
}
