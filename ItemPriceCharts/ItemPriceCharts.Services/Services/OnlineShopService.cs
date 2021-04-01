using System;
using System.Linq;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(OnlineShopService));

        private readonly IRepository<OnlineShop> onlineShopRepository;

        public OnlineShopService(IRepository<OnlineShop> onlineShopRepository)
        {
            this.onlineShopRepository = onlineShopRepository;
        }

        private bool IsShopExisting(string url) =>
            this.onlineShopRepository.GetAll(filter: shop => shop.URL == url).GetAwaiter().GetResult().FirstOrDefault() != null;

        public async Task CreateShop(string shopURL, string shopTitle, UserAccount userAccount)
        {
            try
            {
                if (!this.IsShopExisting(shopURL))
                {
                    var newShop = new OnlineShop(
                        url: shopURL,
                        title: shopTitle);
                    newShop.AddUserAccount(userAccount);

                    var onlineShop = await this.onlineShopRepository.Add(newShop);
                    logger.Debug($"Created shop: '{onlineShop}'.");

                    EventsLocator.ShopAdded.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Can't create shop.");
                throw;
            }
        }
    }
}
