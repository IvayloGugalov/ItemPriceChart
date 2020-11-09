using System;
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
    public class ItemPriceServiceTest
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IRepository<ItemPrice>> itemPriceRepositoryMock;
        private Mock<IRepository<ItemModel>> itemRepositoryMock;

        [SetUp]
        public void Setup()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.itemPriceRepositoryMock = new Mock<IRepository<ItemPrice>>();
            this.itemRepositoryMock = new Mock<IRepository<ItemModel>>();
        }

        [TearDown]
        public void TearDown()
        {
            this.unitOfWorkMock.VerifyAll();
            this.itemRepositoryMock.VerifyAll();
            this.itemPriceRepositoryMock.VerifyAll();
        }

        [Test]
        public void NoOp()
        { }

        [Test]
        public void GetAllPricesForItem_WillSucceed()
        {
            //Arrange
            var item = ModelConstruct.ConstructItem(id: 1);
            var itemPrice = ModelConstruct.ConstructItemPrice(
                id: 1,
                priceDate: DateTime.Now,
                currentPrice: 100,
                itemId: item.Id);

            var itemPriceService = new ItemPriceService(this.unitOfWorkMock.Object);

            this.unitOfWorkMock.SetupGet(_ => _.ItemPriceRepository)
                .Returns(this.itemPriceRepositoryMock.Object);

            MockMethods<ItemPrice>.GetAll(this.itemPriceRepositoryMock, new List<ItemPrice> { itemPrice });

            //Act
            var retrievedPrices = itemPriceService.GetPricesForItem(item.Id);

            //Assert
            Assert.AreEqual(itemPrice, retrievedPrices.First());
        }

        [Test]
        public void CreateItemPrice_WillSucceed()
        {
            //Arrange
            var item = ModelConstruct.ConstructItem(id: 1);
            var itemPrice = ModelConstruct.ConstructItemPrice(
                id: 1,
                priceDate: DateTime.Now,
                currentPrice: 100,
                itemId: item.Id);

            var itemPriceService = new ItemPriceService(this.unitOfWorkMock.Object);

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository).Returns(this.itemRepositoryMock.Object);
            this.unitOfWorkMock.SetupGet(_ => _.ItemPriceRepository).Returns(this.itemPriceRepositoryMock.Object);

            itemPriceRepositoryMock.Setup(_ => _.Add(itemPrice));
            this.itemRepositoryMock.Setup(_ => _.IsExisting(item.Id)).ReturnsAsync(true);

            //Act
            itemPriceService.CreateItemPrice(itemPrice);

            //Assert
            this.unitOfWorkMock.Verify(_ => _.SaveChangesAsync());
        }

        [Test]
        public void GetLatestItemPrice_WillSucceed()
        {
            //Arrange
            var item = ModelConstruct.ConstructItem(id: 1);
            var newestItemPrice = ModelConstruct.ConstructItemPrice(
                id: 1,
                priceDate: DateTime.Now,
                currentPrice: 100,
                itemId: item.Id);
            var itemPrice = ModelConstruct.ConstructItemPrice(
                id: 2,
                priceDate: new DateTime(2000, 1, 15),
                currentPrice: 120,
                itemId: item.Id);

            var itemPriceService = new ItemPriceService(this.unitOfWorkMock.Object);

            this.unitOfWorkMock.SetupGet(_ => _.ItemPriceRepository).Returns(this.itemPriceRepositoryMock.Object);

            MockMethods<ItemPrice>.GetAll(this.itemPriceRepositoryMock,
                new List<ItemPrice> { itemPrice, newestItemPrice }.OrderByDescending(p => p.PriceDate).ToList());

            //Act
            var latestPrice = itemPriceService.GetLatestItemPrice(item.Id);

            //Assert
            Assert.AreEqual(newestItemPrice.Price, latestPrice);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsItemExisting_WithTestCases(bool isExisting)
        {
            //Arrange
            var item = ModelConstruct.ConstructItem(id: 1);
            var itemPriceService = new ItemPriceService(this.unitOfWorkMock.Object);

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository).Returns(this.itemRepositoryMock.Object);
            this.itemRepositoryMock.Setup(_ => _.IsExisting(item.Id)).ReturnsAsync(isExisting);
            //Act
            var isItemExisting = itemPriceService.IsItemExisting(item.Id);

            //Assert
            Assert.AreEqual(isExisting, isItemExisting);
        }
    }
}