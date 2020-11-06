using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemService
    {
        ItemModel GetBy(object id);
        IEnumerable<ItemModel> GetAll(OnlineShopModel onlineShop);
        void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemType type);
        void UpdateItem(ItemModel item);
        bool UpdateItemPrice(ItemModel item, out ItemPrice updatedItemPrice);
        void DeleteItem(ItemModel item);
        bool IsItemExisting(object id);
    }
}
