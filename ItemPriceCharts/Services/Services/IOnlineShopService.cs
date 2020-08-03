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
        void Update(OnlineShopModel onlineShop);
        void Delete(int id);
        bool IsShopExisting(int shopId);
    }
}
