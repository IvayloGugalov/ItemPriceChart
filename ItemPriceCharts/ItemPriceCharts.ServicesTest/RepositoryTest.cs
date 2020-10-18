using System.Threading;

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
        public void GetById_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(id: 1, onlineShop: shop);

            this.dbSetMock = new Mock<DbSet<ItemModel>>();

            this.modelsContextMock.Setup(_ => _.Set<ItemModel>()).Returns(this.dbSetMock.Object);
            this.SetupDatabaseCanConnect(true);
            this.dbSetMock.Setup(_ => _.FindAsync(item.Id)).ReturnsAsync(item);

            //Act
            var repo = new Repository<ItemModel>(this.modelsContextMock.Object);
            var retrievedItem = repo.GetById(1).Result;

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
            var retrievedItem = itemRepository.Add(item);

            //Assert
            this.modelsContextMock.Verify(_ => _.Set<ItemModel>());
            this.dbSetMock.Verify(_ => _.AddAsync(It.Is<ItemModel>(i => i == item), It.IsAny<CancellationToken>()), Times.Once());

        }

        private void SetupDatabaseCanConnect(bool canConnect)
        {
            this.modelsContextMock.SetupGet(_ => _.Database).Returns(this.dbFacade.Object);
            this.dbFacade.Setup(_ => _.CanConnect()).Returns(canConnect);
        }
    }
}
