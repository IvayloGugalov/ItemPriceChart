using Services.Data;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Strategies
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
                if (!this.IsItemExisting(item.ItemtId))
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
                .FirstOrDefault(item => item.ItemtId == id) ?? throw new Exception();
        }

        public void UpdateItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item.ItemtId))
                {
                    var itemToUpdate = this.GetById(item.ItemtId);
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
                if (this.IsItemExisting(item.ItemtId))
                {
                    var itemToDelete = this.GetById(item.ItemtId);
                    this.unitOfWork.ItemRepository.Delete(itemToDelete.ItemtId);
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
                .Any(item => item.ItemtId == itemId);
        }
    }
}
