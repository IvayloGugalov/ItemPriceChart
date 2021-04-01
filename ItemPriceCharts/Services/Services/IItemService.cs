using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemService
    {
        Task CreateItem(string itemURL, OnlineShop onlineShop, ItemType type);
        void UpdateItem(Item item);
        Task<ItemPrice> UpdateItemPrice(Item item);
        Task<bool> DeleteItem(Item item);
    }
}
