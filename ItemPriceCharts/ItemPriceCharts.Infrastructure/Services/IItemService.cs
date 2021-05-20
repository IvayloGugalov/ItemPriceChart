using System.Threading.Tasks;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IItemService
    {
        Task AddItemToShop(string itemURL, OnlineShop onlineShop, ItemType type);
        Task<bool> DeleteItem(Item item);
        Task UpdateItem(Item item);
        Task<ItemPrice> UpdateItemPrice(Item item);
    }
}