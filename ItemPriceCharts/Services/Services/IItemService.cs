using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Strategies
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
