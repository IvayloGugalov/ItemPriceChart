using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Data;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(OnlineShopService));

        private readonly ModelsContext dbContext;

        public OnlineShopService(ModelsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task<bool> IsShopExisting(string url) =>
            await this.dbContext.OnlineShops.SingleOrDefaultAsync(shop => shop.Url == url) != null;

        public async Task CreateShop(UserAccount userAccount, string shopUrl, string shopTitle)
        {
            try
            {
                var isShopExisting = await this.IsShopExisting(shopUrl).ConfigureAwait(false);

                if (!isShopExisting)
                {
                    var newShop = new OnlineShop(
                        url: shopUrl,
                        title: shopTitle);

                    this.dbContext.UserAccounts.Attach(userAccount);

                    userAccount.AddOnlineShop(newShop);

                    await this.dbContext.SaveChangesAsync();

                    Logger.Debug($"Created shop: '{newShop}'.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Can't create shop.");
                throw;
            }
        }
    }
}
