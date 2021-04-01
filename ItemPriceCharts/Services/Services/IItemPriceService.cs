using System.Collections.Generic;
using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemPriceService
    {
        Task<ItemPrice> CreateItemPrice(double price, int itemId);
        Task<IEnumerable<ItemPrice>> GetPricesForItem(int itemId);
        Task<double> GetLatestItemPrice(int itemId);
    }
}
