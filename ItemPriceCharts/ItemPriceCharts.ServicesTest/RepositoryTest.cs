using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.ServicesTest.MockHelper;

namespace ItemPriceCharts.ServicesTest
{
    public class RepositoryTest
    {
        private Mock<ModelsContext> modelsContextMock;
        private Mock<DbSet<ItemModel>> dbSetMock;
        private Mock<DatabaseFacade> dbFacade;

        [SetUp]
        public void Setup()
        {
            this.modelsContextMock = new Mock<ModelsContext>();
            this.dbFacade = new Mock<DatabaseFacade>(this.modelsContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.modelsContextMock.VerifyAll();
            this.dbFacade.VerifyAll();
            this.dbSetMock.VerifyAll();
        }

        [Test]
        public void NoOp()
        { }

        [Test]
        public void FindAsync_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(id: 1, onlineShop: shop);

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);
            this.dbSetMock.Setup(_ => _.FindAsync(item.Id)).ReturnsAsync(item);

            //Act
            var itemRepository = new Repository<ItemModel>(this.modelsContextMock.Object);
            var retrievedItem = itemRepository.FindAsync(1).Result;

            //Assert
            Assert.AreEqual(item, retrievedItem);
        }

        [Test]
        public void Add_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(id: 1, onlineShop: shop);

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);

            //Act
            var itemRepository = new Repository<ItemModel>(this.modelsContextMock.Object);
            itemRepository.Add(item);

            //Assert
            this.modelsContextMock.Verify(_ => _.Set<ItemModel>());
            this.dbSetMock.Verify(_ => _.Add(It.Is<ItemModel>(i => i == item)), Times.Once());
        }

        [Test]
        public void Update_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(id: 1, onlineShop: shop, price: 20);

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);

            //Act
            var itemRepository = new Repository<ItemModel>(this.modelsContextMock.Object);
            itemRepository.Update(item);

            //Assert
            this.modelsContextMock.Verify(_ => _.Set<ItemModel>());
            this.dbSetMock.Verify(_ => _.Update(It.Is<ItemModel>(i => i == item)), Times.Once());
        }

        [Test]
        public void Delete_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(id: 1, onlineShop: shop, price: 20);

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);
            this.modelsContextMock.Setup(_ => _.GetEntityState(item)).Returns(EntityState.Unchanged);

            //Act
            var itemRepository = new Repository<ItemModel>(this.modelsContextMock.Object);
            itemRepository.Delete(item);

            //Assert
            this.modelsContextMock.Verify(_ => _.Set<ItemModel>());
            this.dbSetMock.Verify(_ => _.Remove(It.Is<ItemModel>(i => i == item)), Times.Once());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsExisting_WillSucceed(bool isFound)
        {
            //Arrange
            var itemId = 1;
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = isFound ?
                ModelConstruct.ConstructItem(id: itemId, onlineShop: shop, price: 20) : null;

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);
            this.dbSetMock.Setup(_ => _.FindAsync(itemId)).ReturnsAsync(item);

            //Act
            var itemRepository = new Repository<ItemModel>(this.modelsContextMock.Object);
            var isExisting = itemRepository.IsExisting(itemId).Result;

            //Assert
            this.modelsContextMock.Verify(_ => _.Set<ItemModel>());
            this.dbSetMock.Verify(_ => _.FindAsync(It.Is<int>(i => i == itemId)), Times.Once());
            Assert.AreEqual(isFound, isExisting);
        }

        private void SetupDatabaseCanConnect(bool canConnect)
        {
            this.modelsContextMock.SetupGet(_ => _.Database).Returns(this.dbFacade.Object);
            this.dbFacade.Setup(_ => _.CanConnect()).Returns(canConnect);
        }
    }
}
