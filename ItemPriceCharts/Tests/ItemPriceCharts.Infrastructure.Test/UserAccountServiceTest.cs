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
    public class UserAccountServiceTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateUserAccount_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var firstName = "first";
            var lastName = "last";
            var email = new Email("userEmail@mail.com");
            var userName = "user name";
            var password = "the password";

            var userAccountService = new UserAccountService(dbContext);

            var result = await userAccountService.CreateUserAccount(
                firstName: firstName,
                lastName: lastName,
                email: email.Value,
                userName: userName,
                password: password);

            Assert.AreEqual(result, UserAccountRegistrationResult.UserAccountCreated);

            // Make sure we have the same entity in the DB
            dbContext.ChangeTracker.Clear();

            var userAccountFromDb = dbContext.UserAccounts.First();

            Assert.AreEqual(firstName, userAccountFromDb.FirstName);
            Assert.AreEqual(lastName, userAccountFromDb.LastName);
            Assert.AreEqual(email, userAccountFromDb.Email);
            Assert.AreEqual(userName, userAccountFromDb.Username);
            Assert.AreEqual(password, userAccountFromDb.Password);
        }

        [Test]
        public async Task DeleteUserAccount_WillSucceed()
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

            var onlineShop = new OnlineShop("www.url.com", "shop title");

            dbContext.UserAccountOnlineShops.Add(new UserAccountOnlineShops(userAccount, onlineShop));
            dbContext.SaveChanges();

            var userAccountService = new UserAccountService(dbContext);

            var isDeleted = await userAccountService.DeleteUserAccount(userAccount);

            Assert.IsTrue(isDeleted);

            // Make sure we have the entity deleted from the DB
            dbContext.ChangeTracker.Clear();

            var userAccountOnlineShopFromDb = dbContext.UserAccountOnlineShops.FirstOrDefault();
            var userAccountFromDb = dbContext.UserAccounts.FirstOrDefault();

            // Should this be null as well?
            //var onlineShopFromDb = dbContext.OnlineShops.FirstOrDefault();

            Assert.IsNull(userAccountOnlineShopFromDb);
            Assert.IsNull(userAccountFromDb);
        }

        [Test]
        public async Task GetUserAccount_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var userName = "user name";
            var emailValue = "userEmail@mail.com";

            var userAccount = new UserAccount(
                firstName: "first",
                lastName: "last",
                email: new Email(emailValue),
                userName: userName,
                password: "the password");

            dbContext.UserAccounts.Add(userAccount);
            dbContext.SaveChanges();

            var userAccountService = new UserAccountService(dbContext);

            var retrievedUser = await userAccountService.GetUserAccount(userName, emailValue);

            Assert.AreEqual(userAccount, retrievedUser);
        }

        [Test]
        public async Task TryGetUserAccount_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var userName = "user name";
            var emailValue = "userEmail@mail.com";
            var password = "the password";

            var userAccount = new UserAccount(
                firstName: "first",
                lastName: "last",
                email: new Email(emailValue),
                userName: userName,
                password: password);

            dbContext.UserAccounts.Add(userAccount);
            dbContext.SaveChanges();

            var userAccountService = new UserAccountService(dbContext);

            var (loginResult, retrievedAccount) = await userAccountService.TryGetUserAccount(userName, emailValue, password);

            Assert.AreEqual(UserAccountLoginResult.SuccessfulLogin, loginResult);
            Assert.AreEqual(userAccount, retrievedAccount);
        }
    }
}