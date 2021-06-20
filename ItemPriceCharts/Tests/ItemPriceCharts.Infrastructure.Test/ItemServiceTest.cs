using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Moq;
using NUnit.Framework;
using TestSupport.EfHelpers;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.Infrastructure.Services;

namespace ItemPriceCharts.Infrastructure.Test
{
    [TestFixture]
    public class ItemServiceTest
    {
        private Mock<IHtmlWebWrapper> htmlWebWrapperMock;
        private Mock<IItemDataRetrieveService> itemDataRetrieveServiceMock;

        [SetUp]
        public void Setup()
        {
            this.htmlWebWrapperMock = new Mock<IHtmlWebWrapper>();
            this.itemDataRetrieveServiceMock = new Mock<IItemDataRetrieveService>();
        }

        [TearDown]
        public void CleanUp()
        {
            this.itemDataRetrieveServiceMock.VerifyAll();
            this.htmlWebWrapperMock.VerifyAll();
        }

        [Test]
        public async Task AddItemToShop_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var onlineShop = new OnlineShop("www.url.com", "shop title");
            var itemUrl = "www.url/item.com";
            var title = "item title";
            var description = "some description";
            var price = 20.5;

            dbContext.OnlineShops.Add(onlineShop);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();
            
            var itemService = new ItemService(dbContext, this.htmlWebWrapperMock.Object, this.itemDataRetrieveServiceMock.Object);

            this.htmlWebWrapperMock.Setup(_ => _.LoadDocument(itemUrl));
            this.itemDataRetrieveServiceMock.Setup(_ => _.CreateItem(It.IsAny<HtmlDocument>(), onlineShop.Title))
                .Returns((title, description, price));

            await itemService.AddItemToShop(itemUrl, onlineShop, ItemType.ComputerItem);

            // Make sure we have the same entity in the DB
            dbContext.ChangeTracker.Clear();
            var itemFromDb = dbContext.Items.First();

            Assert.AreEqual(1, onlineShop.Items.Count);
            Assert.AreEqual(onlineShop.Items.First(), itemFromDb);
        }

        [Test]
        public async Task DeleteItem_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var onlineShop = new OnlineShop("www.url.com", "shop title");
            var itemUrl = "www.url/item.com";
            var title = "item title";
            var description = "some description";
            var price = 20.5;
            var item = new Item(itemUrl, title, description, price, onlineShop, ItemType.ComputerItem);

            dbContext.Items.Add(item);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            var itemService = new ItemService(dbContext, this.htmlWebWrapperMock.Object, this.itemDataRetrieveServiceMock.Object);

            var deleteIsSuccessful = await itemService.DeleteItem(item);

            Assert.IsTrue(deleteIsSuccessful);
            // Make sure the entity is not in the DB
            dbContext.ChangeTracker.Clear();
            var onlineShopFromDb = dbContext.OnlineShops.First();

            Assert.AreEqual(0, dbContext.Items.Count());
            Assert.IsNull(onlineShopFromDb.Items.FirstOrDefault());
        }

        [Test]
        public async Task UpdateItemPrice_WillSucceed()
        {
            var options = SqliteInMemory.CreateOptions<ModelsContext>();
            using var dbContext = new ModelsContext(options);
            dbContext.Database.EnsureCreated();

            var onlineShop = new OnlineShop("www.url.com", "shop title");
            var itemUrl = "www.url/item.com";
            var title = "item title";
            var description = "some description";
            var priceValue = 20.5;
            var newPriceValue = 10.2;

            var item = new Item(itemUrl, title, description, priceValue, onlineShop, ItemType.ComputerItem);

            dbContext.Items.Add(item);
            dbContext.SaveChanges();
            dbContext.ChangeTracker.Clear();

            this.htmlWebWrapperMock.Setup(_ => _.LoadDocument(itemUrl));
            this.itemDataRetrieveServiceMock.Setup(_ => _.CreateItem(It.IsAny<HtmlDocument>(), onlineShop.Title))
                .Returns((title, description, newPriceValue));

            var itemService = new ItemService(dbContext, this.htmlWebWrapperMock.Object, this.itemDataRetrieveServiceMock.Object);

            var updatedItemPrice = await itemService.TryUpdateItemPrice(item);
            
            Assert.AreEqual(newPriceValue, updatedItemPrice.Price);
            // Make sure we have the updated entity in the DB
            dbContext.ChangeTracker.Clear();
            var itemFromDb = dbContext.Items.First();

            Assert.AreEqual(newPriceValue, itemFromDb.CurrentPrice.Price);
        }
    }
}