using System.Collections.Generic;
using System.Threading.Tasks;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemPriceService
    {
        void CreateItemPrice(ItemPrice itemPrice);
        Task<IEnumerable<ItemPrice>> GetPricesForItem(int itemId);
        double GetLatestItemPrice(int itemId);
    }
}
