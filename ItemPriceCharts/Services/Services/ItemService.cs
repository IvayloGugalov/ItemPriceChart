using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Strategies
{
    public class ItemService : IItemService
    {
        private readonly UnitOfWork unitOfWork;

        public ItemService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void AddItem(ItemModel item)
        {
            try
            {
                if (!this.IsItemExisting(item.Id))
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

        public ItemModel GetById(int id)
        {
            return this.unitOfWork.ItemRepository.All()
                .FirstOrDefault(item => item.Id == id) ?? throw new Exception();
        }

        public void UpdateItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    var itemToUpdate = this.GetById(item.Id);
                    itemToUpdate.Price = item.Price;
                    itemToUpdate.Title = item.Title;
                    itemToUpdate.Description = item.Description;

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
                if (this.IsItemExisting(item.Id))
                {
                    var itemToDelete = this.GetById(item.Id);
                    this.unitOfWork.ItemRepository.Delete(itemToDelete.Id);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ItemModel> GetAll()
        {
            return this.unitOfWork.ItemRepository.All();
        }

        private bool IsItemExisting(int itemId)
        {
            return this.unitOfWork.ItemRepository.All()
                .Any(item => item.Id == itemId);
        }
    }
}
