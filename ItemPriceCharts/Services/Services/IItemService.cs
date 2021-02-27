using System.Collections.Generic;
using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemService
    {
        Item FindItem(int id);
        IEnumerable<Item> GetItemsForShop(OnlineShop onlineShop);
        Task<IEnumerable<Item>> GetAllItems();
        Task CreateItem(string itemURL, OnlineShop onlineShop, ItemType type);
        void UpdateItem(Item item);
        Task<ItemPrice> UpdateItemPrice(Item item);
        Task<bool> DeleteItem(Item item);
        bool IsItemExisting(int id);
    }
}
