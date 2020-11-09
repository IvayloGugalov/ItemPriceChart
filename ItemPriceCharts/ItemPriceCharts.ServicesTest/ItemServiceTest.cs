using System.Collections.Generic;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.ServicesTest.MockHelper;
using HtmlAgilityPack;
using System;

namespace ItemPriceCharts.ServicesTest
{
    public class ItemServiceTest
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IItemPriceService> itemPriceService;
        private Mock<IRepository<ItemModel>> itemRepositoryMock;
        private Mock<HtmlWeb> htmlWebMock;

        [SetUp]
        public void Setup()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.itemPriceService = new Mock<IItemPriceService>();
            this.itemRepositoryMock = new Mock<IRepository<ItemModel>>();
            this.htmlWebMock = new Mock<HtmlWeb>();
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

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository)
                .Returns(this.itemRepositoryMock.Object);

            MockMethods<ItemModel>.GetAll(this.itemRepositoryMock, new List<ItemModel> { item });

            //Act
            var itemService = new ItemService(this.unitOfWorkMock.Object, this.itemPriceService.Object);
            var retrievedItems = itemService.GetAllItemsForShop(shop);

            //Assert
            Assert.AreEqual(item, retrievedItems.First());
        }

        [Test]
        public void FindItem_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(
                id: 1,
                onlineShop: shop);

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository)
                .Returns(this.itemRepositoryMock.Object);

            this.itemRepositoryMock.Setup(_ => _.FindAsync(item.Id)).ReturnsAsync(item);

            //Act
            var itemService = new ItemService(this.unitOfWorkMock.Object, this.itemPriceService.Object);
            var retrievedItem = itemService.FindItem(item.Id);

            //Assert
            Assert.AreEqual(item, retrievedItem);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsItemExisting_WillSucceed(bool isFound)
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(
                id: 1,
                onlineShop: shop);

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository)
                .Returns(this.itemRepositoryMock.Object);

            this.itemRepositoryMock.Setup(_ => _.IsExisting(item.Id)).ReturnsAsync(isFound);

            //Act
            var itemService = new ItemService(this.unitOfWorkMock.Object, this.itemPriceService.Object);
            var isExisting = itemService.IsItemExisting(item.Id);

            //Assert
            Assert.AreEqual(isFound, isExisting);
        }

        [Test]
        public void Create_WillSucceed()
        {
            //Arrange
            var shop = ModelConstruct.ConstructOnlineShop(id: 1);
            var item = ModelConstruct.ConstructItem(
                id: 1,
                url: @"https:\\vario.com\item",
                itemType: ItemType.ComputerItem,
                title: "Item",
                descritpion: "description",
                onlineShop: shop);
            var itemPrice = ModelConstruct.ConstructItemPrice(
                id: 1,
                priceDate: DateTime.UtcNow,
                currentPrice: 50,
                itemId: item.Id);
            var htmlDocument = new HtmlDocument();

            this.unitOfWorkMock.SetupGet(_ => _.ItemRepository)
                .Returns(this.itemRepositoryMock.Object);

            MockMethods<ItemModel>.GetAll(this.itemRepositoryMock, new List<ItemModel>());
            this.itemRepositoryMock.Setup(_ => _.Add(item));
            this.itemPriceService.Setup(_ => _.CreateItemPrice(itemPrice));

            //Act
            var itemService = new ItemService(this.unitOfWorkMock.Object, this.itemPriceService.Object);
            itemService.CreateItem(item.URL, shop, item.Type);

            //Assert
            this.unitOfWorkMock.Verify(_ => _.SaveChangesAsync());
        }
    }
}
