using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemService
    {
        Item FindItem(int id);
        IEnumerable<Item> GetItemsForShop(OnlineShop onlineShop);
        IEnumerable<Item> GetAllItems();
        void CreateItem(string itemURL, OnlineShop onlineShop, ItemType type);
        void UpdateItem(Item item);
        bool UpdateItemPrice(Item item, out ItemPrice updatedItemPrice);
        void DeleteItem(Item item);
        bool IsItemExisting(int id);
    }
}
