using System;
using System.Collections.Generic;
using System.Threading;

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

            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

            this.item = ItemExtension.ConstructItem(
                id: 1,
                url: string.Concat(onlineShop.URL, @"/firstItem"),
                title: "firstItem",
                description: "item description",
                price: 20.5,
                onlineShop: onlineShop,
                type: ItemType.ComputerItem);

            this.exceptionThrown = new Exception();

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
        public void LoadItemPriceInformation_WillLoadData_Correctly()
        {
            var itemPricesCollection = new List<ItemPrice>()
            {
                new ItemPrice(15, this.item.Id),
                new ItemPrice(16, this.item.Id),
                new ItemPrice(20.5, this.item.Id),
            };

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .ReturnsAsync(itemPricesCollection);

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            Assert.AreEqual(itemPricesCollection.Count, this.itemInformationViewModel.LineSeries.Values.Count);
        }

        [Test]
        public void LoadItemPriceInformation_WhenExceptionThrown_WillShowMessageDialog()
        {

            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .Throws(this.exceptionThrown);

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual($"Could not load price information for {this.item.Title}", messageDialogViewModel.Description);
        }

        [Test]
        public void UpdatePriceAction_WillGetNewPrice_UpdatesChart()
        {
            var updatedItemPrice = new ItemPrice(21, this.item.Id);

            this.itemPriceServiceMock.Setup(_ => _.GetPricesForItem(this.item.Id))
                .ReturnsAsync(new List<ItemPrice>());
            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .ReturnsAsync(updatedItemPrice);


            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);
            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.AreEqual(1, this.itemInformationViewModel.Labels.Count);
            Assert.AreEqual(1, this.itemInformationViewModel.LineSeries.Values.Count);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public void UpdatePriceAction_WithNoNewPrice_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .ReturnsAsync(It.IsAny<ItemPrice>());

            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual("The price of the item hasn't been changed.", messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public void UpdatePriceAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };
            var expectedOnExceptionDialogMessage = $"Could not update price for {this.item.Title}";

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemPriceServiceMock.Object, this.itemServiceMock.Object, this.item);

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .Throws(this.exceptionThrown);

            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }
    }
}
