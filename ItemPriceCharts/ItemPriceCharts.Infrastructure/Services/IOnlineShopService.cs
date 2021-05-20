using System.Threading.Tasks;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IOnlineShopService
    {
        Task CreateShop(string shopURL, string shopTitle);
    }
}