using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IWebPageService
    {
        void CreateItem(string itemURL, OnlineShopModel onlineShop);

        void DeleteShop(OnlineShopModel onlineShop);

        IEnumerable<ItemModel> RetrieveItemsForShop();

        IEnumerable<OnlineShopModel> RetrieveOnlineShops();
    }
}