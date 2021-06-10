using System.Threading.Tasks;
using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IOnlineShopService
    {
        Task CreateShop(UserAccount userAccount, string shopUrl, string shopTitle);
    }
}