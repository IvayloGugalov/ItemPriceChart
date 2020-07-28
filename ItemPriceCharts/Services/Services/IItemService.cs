using System;
using System.Collections.Generic;
using System.Text;
using Services.Models;

namespace Services.Strategies
{
    public interface IItemService
    {
        ItemModel GetById(int id);
        IEnumerable<ItemModel> GetAll();
        void AddItem(ItemModel item);
        void UpdateItem(ItemModel item);
        void DeleteItem(ItemModel item);
    }
}
