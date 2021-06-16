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

                    await dbContext.OnlineShops.AddAsync(newShop).ConfigureAwait(false);

                    // Always save before creating the link entity in the Join table
                    await dbContext.SaveChangesAsync();

                    // # Will not work:
                        //dbContext.UserAccountOnlineShops.Add(new UserAccountOnlineShops(userAccount, newShop));
                            // ---> Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 19: 'UNIQUE constraint failed: OnlineShops.Id'.
                            

                    // # This will work, but it needs to get the entities from the database to start tracking them....
                        //var user = dbContext.UserAccounts
                        //    .Include(p => p.OnlineShopsForUser)
                        //    .Single(p => p.Id == userAccount.Id);

                        //var shop = dbContext.OnlineShops.Include(x => x.UserAccounts)
                        //    .Single(x => x.Id == newShop.Id);

                        //user.AddOnlineShop(shop);

                    await dbContext.Database.ExecuteSqlRawAsync(
                        @"INSERT INTO UserAccountOnlineShops (UserAccountId, OnlineShopId) VALUES({0}, {1})", userAccount.Id, newShop.Id)
                        .ConfigureAwait(false);

                    await dbContext.SaveChangesAsync();

                    userAccount.AddOnlineShop(newShop);

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
