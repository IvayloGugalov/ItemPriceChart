using System.Collections.Generic;
using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IOnlineShopService
    {
        OnlineShop FindShop(int id);
        Task<IEnumerable<OnlineShop>> GetAllShops();
        Task CreateShop(string shopURL, string shopTitle);
        void UpdateShop(OnlineShop onlineShop);
        void DeleteShop(OnlineShop onlineShop);
        bool IsShopExisting(int shopId);
    }
}
