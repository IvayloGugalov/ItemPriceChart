using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IWebPageService
    {
        void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemModel.ItemType type);
        void CreateShop(string shopURL, string shopTitle);

        void DeleteShop(OnlineShopModel onlineShop);
        void DeleteItem(ItemModel item);

        IEnumerable<OnlineShopModel> RetrieveOnlineShops();
        IEnumerable<ItemModel> RetrieveItemsForShop(OnlineShopModel onlineShop);

        bool TryGetShop(OnlineShopModel onlineShop);
    }
}