using System;

using NUnit.Framework;
using Moq;
using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Services;
using Autofac.Extras.Moq;
using ItemPriceCharts.Services.Models;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ItemPriceCharts.ServicesTest.MockHelper;
using Microsoft.EntityFrameworkCore.Infrastructure;


using ItemPriceCharts.ServicesTest.MockHelper;

namespace ItemPriceCharts.ServicesTest
{
    public class ItemPriceServiceTest
    {
        private AutoMock mock;
        private ItemPriceService itemPriceService;
        private Mock<UnitOfWork> unitOfWorkMock;
        private Mock<ModelsContext> modelsContextMock;

        [SetUp]
        public void Setup()
        {
            //this.modelsContextMock = new Mock<ModelsContext>();
            //var items = new List<ItemModel> { new ItemModel(1, null, null, null, 0, null, ItemType.ComputerItem) };
            //this.modelsContextMock.Setup(x => x.Items).Returns(DbSetMock.GetQueryableMockDbSet(items));
            //var unit = new UnitOfWork(this.modelsContextMock.Object);

           
            
        }

        [Test]
        public void NoOp()
        { }

        [Test]
        public void CreatingUnitOfWork_WhenSuccessful_WillHaveRepositoryInstances()
        {
            //Assert.IsNotNull(this.unitOfWorkMock.Object.ItemPriceRepository);
            //Assert.IsNotNull(this.unitOfWorkMock.Object.ItemRepository);
            //Assert.IsNotNull(this.unitOfWorkMock.Object.OnlineShopRepository);

            var shop = new OnlineShopModel(id: 1, url: "www.shop.com", title: "shop");
            var itemPrice = new ItemPrice(
                id: 1,
                priceDate: DateTime.Now,
                currentPrice: 100,
                itemId: 1);
            var items = new List<ItemModel>
            {
                new ItemModel(
                    id: 1,
                    url: "www.shop.com/item",
                    title: "Item",
                    description: "Description",
                    price: 10,
                    onlineShop: shop,
                    ItemType.ComputerItem)
            };
            //var itemsDb = DbSetMock.GetQueryableMockDbSet(items);

            this.modelsContextMock = new Mock<ModelsContext>();
            //this.modelsContextMock.Setup(_ => _.Items).Returns(itemsDb);

            var item = new ItemModel(1, null, null, null, 0, null, ItemType.ComputerItem);
            var unit = new Mock<IUnitOfWork>();
            var repoPrice = new Mock<IRepository<ItemPrice>>();

            this.itemPriceService = new ItemPriceService(unit.Object);

            MockMethods.GetAll(repoPrice)
            repoPrice.

            unit.SetupGet(_ => _.ItemPriceRepository).Returns(repoPrice.Object);
            repoPrice.Setup(_ => _.All(
                It.IsAny<Expression<Func<ItemPrice, bool>>>(),
                It.IsAny<Func<IQueryable<ItemPrice>, IOrderedQueryable<ItemPrice>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<ItemPrice> { itemPrice });

            var a = this.itemPriceService.GetPricesForItem(item.Id);
            Assert.AreEqual(itemPrice, a.First());

        }

        [Test]
        public void Test1()
        {
            var itemPrice = new ItemPrice(
                id: 1,
                priceDate: DateTime.Now,
                currentPrice: 100,
                itemId: 1);

            var item = new ItemModel(1, null, null, null, 0, null, ItemType.ComputerItem);
            var repo = new Mock<IRepository<ItemModel>>();
            var unit = new Mock<IUnitOfWork>();
            var repoPrice = new Mock<IRepository<ItemPrice>>();

            this.itemPriceService = new ItemPriceService(unit.Object);

            unit.SetupGet(_ => _.ItemRepository).Returns(repo.Object);
            unit.SetupGet(_ => _.ItemPriceRepository).Returns(repoPrice.Object);
            repo.Setup(_ => _.All(
                It.IsAny<Expression<Func<ItemModel, bool>>>(),
                It.IsAny<Func<IQueryable<ItemModel>, IOrderedQueryable<ItemModel>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<ItemModel> { item });
            repoPrice.Setup(_ => _.Add(itemPrice));
            this.itemPriceService.CreateItemPrice(itemPrice);
            unit.Verify(_ => _.SaveChanges());
        }
    }
}