using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using TestSupport.EfHelpers;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.Infrastructure.Services;

namespace ItemPriceCharts.Infrastructure.Test
{
    [TestFixture]
    public class OnlineShopServiceTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateShop_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var userAccount = new UserAccount(
                firstName: "first",
                lastName: "last",
                email: new Email("userEmail@mail.com"),
                userName: "user name",
                password: "the password");

            var shopUrl = "https://www.urlshop.com";
            var shopTitle = "Title";

            dbContext.UserAccounts.Add(userAccount);
            dbContext.SaveChanges();

            var onlineShopService = new OnlineShopService(dbContext);

            await onlineShopService.CreateShop(userAccount, shopUrl, shopTitle);

            // Make sure we have the same entities in the DB
            dbContext.ChangeTracker.Clear();

            var userAccountFromDb = dbContext.UserAccounts.First();
            var onlineShopFromDb = dbContext.OnlineShops.First();
            var userAccountOnlineShopEntityFromDb = dbContext.UserAccountOnlineShops.First();

            Assert.AreEqual(userAccountFromDb, userAccountOnlineShopEntityFromDb.UserAccount);
            Assert.AreEqual(onlineShopFromDb, userAccountOnlineShopEntityFromDb.OnlineShop);
        }
    }
}