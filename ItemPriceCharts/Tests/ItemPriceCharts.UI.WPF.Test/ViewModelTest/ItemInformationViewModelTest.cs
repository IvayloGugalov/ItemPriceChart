using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ItemInformationViewModelTest
    {
        private ItemInformationViewModel itemInformationViewModel;

        private Mock<IItemPriceService> itemPriceServiceMock;
        private Mock<IItemService> itemServiceMock;

        private Item item;
        private Exception exceptionThrown;

        [SetUp]
        public void SetUp()
        {
            this.itemPriceServiceMock = new Mock<IItemPriceService>();
            this.itemServiceMock = new Mock<IItemService>();

            var shopUrl = "https://www.someShop.com";
            var onlineShop = OnlineShopExtension.ConstructOnlineShop(
                id: 1,
                url: shopUrl,
                title: "someShop");

            this.item = ItemExtension.ConstructItem(
                id: 1,
                url: string.Concat(shopUrl, @"/firstItem"),
                title: "firstItem",
                description: "item description",
                price: 20.5,
                onlineShop: onlineShop,
                type: ItemType.ComputerItem);

            this.exceptionThrown = new Exception("New exception thrown");

            //var a = Mock.Of<Func<MessageDialogViewModel, bool?>>();
            //var command = (RelayCommand<object>)this.itemInformationViewModel.UpdatePriceCommand;
        }

        [TearDown]
        public void TearDown()
        {
            this.itemPriceServiceMock.VerifyAll();
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public async Task LoadItemPriceInformation_WillLoadData_Correctly()
        {
            var itemPricesCollection = new List<ItemPrice>()
            {
                new ItemPrice(15, this.item.Id),
                new ItemPrice(16, this.item.Id),
                new ItemPrice(20.5, this.item.Id),
            };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .Returns(itemPricesCollection);

            await this.itemInformationViewModel.LoadItemPriceInformation();

            Assert.AreEqual(itemPricesCollection.Count, this.itemInformationViewModel.LineSeries.Values.Count);
        }

        [Test]
        public async Task LoadItemPriceInformation_WhenExceptionThrown_WillShowMessageDialog()
        {

            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .Throws(this.exceptionThrown);

            await this.itemInformationViewModel.LoadItemPriceInformation();

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(this.exceptionThrown.Message, messageDialogViewModel.Description);
        }

        [Test]
        public async Task UpdatePriceAction_WillGetNewPrice_UpdatesChart()
        {
            var updatedItemPrice = new ItemPrice(21, this.item.Id);

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .Returns(new List<ItemPrice>());
            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .Returns(updatedItemPrice);

            await this.itemInformationViewModel.LoadItemPriceInformation();

            await this.itemInformationViewModel.UpdatePriceCommand.ExecuteAsync();

            Assert.AreEqual(1, this.itemInformationViewModel.Labels.Count);
            Assert.AreEqual(1, this.itemInformationViewModel.LineSeries.Values.Count);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public async Task UpdatePriceAction_WithNoNewPrice_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .Returns(It.IsAny<ItemPrice>());

            await this.itemInformationViewModel.UpdatePriceCommand.ExecuteAsync();

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual("The price of the item hasn't been changed.", messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public async Task UpdatePriceAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .Throws(this.exceptionThrown);

            await this.itemInformationViewModel.UpdatePriceCommand.ExecuteAsync();

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(this.exceptionThrown.Message, messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }
    }
}
