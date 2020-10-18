using System.Collections.Generic;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.ServicesTest.MockHelper;

namespace ItemPriceCharts.ServicesTest
{
    public class ItemServiceTest
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IItemPriceService> itemPriceService;
        private Mock<IRepository<ItemModel>> itemRepositoryMock;

        [SetUp]
        public void Setup()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.itemPriceService = new Mock<IItemPriceService>();
            this.itemRepositoryMock = new Mock<IRepository<ItemModel>>();
        }

        [TearDown]
        public void TearDown()
        {
            this.unitOfWorkMock.VerifyAll();
            this.itemRepositoryMock.VerifyAll();
            this.itemPriceService.VerifyAll();
        }

        [Test]
        public void NoOp()
        { }

        [Test]
        public void GetAll_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(
                id: 1,
                onlineShop: shop);

            var itemService = new ItemService(this.unitOfWorkMock.Object, this.itemPriceService.Object); ;

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository)
                .Returns(this.itemRepositoryMock.Object);

            MockMethods<ItemModel>.GetAll(this.itemRepositoryMock, new List<ItemModel> { item });

            //Act
            var retrievedItems = itemService.GetAll(shop);

            //Assert
            Assert.AreEqual(item, retrievedItems.First());
        }
    }
}
