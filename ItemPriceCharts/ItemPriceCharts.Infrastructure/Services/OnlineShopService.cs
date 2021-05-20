using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(OnlineShopService));

        private async Task<bool> IsShopExisting(ModelsContext dbContext, string url) =>
            await dbContext.OnlineShops.Where(shop => shop.URL == url).SingleOrDefaultAsync() != null;

        public async Task CreateShop(string shopURL, string shopTitle)
        {
            try
            {
                using ModelsContext dbContext = new();

                var isShopExisting = await this.IsShopExisting(dbContext, shopURL).ConfigureAwait(false);

                if (!isShopExisting)
                {
                    var newShop = new OnlineShop(
                        url: shopURL,
                        title: shopTitle);


                    var onlineShop = await dbContext.OnlineShops.AddAsync(newShop);

                    logger.Debug($"Created shop: '{onlineShop}'.");
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
