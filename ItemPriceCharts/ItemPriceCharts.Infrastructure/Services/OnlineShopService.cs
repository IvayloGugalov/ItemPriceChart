using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Events;
using ItemPriceCharts.Infrastructure.Data;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(OnlineShopService));

        private async Task<bool> IsShopExisting(ModelsContext dbContext, string url) =>
            await dbContext.OnlineShops.SingleOrDefaultAsync(shop => shop.Url == url) != null;

        public async Task CreateShop(UserAccount userAccount, string shopUrl, string shopTitle)
        {
            try
            {
                using ModelsContext dbContext = new();

                var isShopExisting = await this.IsShopExisting(dbContext, shopUrl).ConfigureAwait(false);

                if (!isShopExisting)
                {
                    var newShop = new OnlineShop(
                        url: shopUrl,
                        title: shopTitle);

                    dbContext.BeginTransaction();

                    var userAccountOnlineShop = userAccount.AddOnlineShop(newShop);
                    var onlineShop = await dbContext.OnlineShops.AddAsync(newShop);

                    

                    await dbContext.UserAccountOnlineShops.AddAsync(userAccountOnlineShop);

                    dbContext.CommitToDatabase();

                    DomainEvents.ShopAdded.Raise(onlineShop.Entity);

                    Logger.Debug($"Created shop: '{onlineShop}'.");
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
