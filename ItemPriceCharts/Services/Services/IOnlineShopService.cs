using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IOnlineShopService
    {
        Task CreateShop(string shopURL, string shopTitle, UserAccount userAccount);
    }
}
