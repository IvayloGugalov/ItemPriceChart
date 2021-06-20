using System.Threading.Tasks;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IItemService
    {
        Task AddItemToShop(string itemUrl, OnlineShop onlineShop, ItemType type);
        Task<bool> DeleteItem(Item item);
        Task UpdateItem(Item item);
        Task<ItemPrice> TryUpdateItemPrice(Item item);
    }
}