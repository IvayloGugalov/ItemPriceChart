using System.Collections.Generic;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.Services.Strategies
{
    public interface IOnlineShopService
    {
        OnlineShopModel GetById(int id);
        IEnumerable<OnlineShopModel> GetAll();
        DatabaseResult AddShop(OnlineShopModel onlineShop);
        void UpdateShop(OnlineShopModel onlineShop);
        void DeleteShop(OnlineShopModel onlineShop);
        bool IsShopExisting(int shopId);
    }
}
