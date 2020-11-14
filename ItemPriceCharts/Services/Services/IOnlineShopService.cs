using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IOnlineShopService
    {
        OnlineShop FindShop(int id);
        IEnumerable<OnlineShop> GetAllShops();
        void CreateShop(string shopURL, string shopTitle);
        void UpdateShop(OnlineShop onlineShop);
        void DeleteShop(OnlineShop onlineShop);
        bool IsShopExisting(int shopId);
    }
}
