using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IItemPriceService
    {
        void CreateItemPrice(ItemPrice itemPrice);
        IEnumerable<ItemPrice> GetPricesForItem(int itemId);
        double GetLatestItemPrice(int itemId);

    }
}
