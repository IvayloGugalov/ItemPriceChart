using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Strategies
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ItemModel GetById(int id) =>
            this.unitOfWork.ItemRepository.All(item => item.Id == id).Result
                .FirstOrDefault() ?? throw new Exception();

        public void AddItem(ItemModel item)
        {
            try
            {
                if (!this.IsItemExisting(item))
                {
                    this.unitOfWork.ItemRepository.Add(item);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item))
                {
                    var itemToUpdate = this.GetById(item.Id);
                    itemToUpdate.CurrentPrice = item.CurrentPrice;
                    itemToUpdate.Title = item.Title;
                    itemToUpdate.Description = item.Description;
                    this.unitOfWork.ItemRepository.Update(itemToUpdate);

                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item))
                {
                    this.unitOfWork.ItemRepository.Delete(item);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ItemModel> GetAll(OnlineShopModel onlineShop) =>
            this.unitOfWork.ItemRepository.All(filter: item => item.OnlineShop.Id == onlineShop.Id).Result;

        public bool IsItemExisting(ItemModel item) =>
            this.unitOfWork.ItemRepository.All(i => i.URL == item.URL || i.Id == item.Id).Result.Any();
    }
}
