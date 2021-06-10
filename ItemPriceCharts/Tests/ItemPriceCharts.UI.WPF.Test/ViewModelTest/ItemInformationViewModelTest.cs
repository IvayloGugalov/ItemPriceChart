using System;
using System.Collections.Generic;
using System.Threading;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ItemInformationViewModelTest
    {
        private ItemInformationViewModel itemInformationViewModel;

        private Mock<IItemService> itemServiceMock;

        private Item item;

        [SetUp]
        public void SetUp()
        {
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

            this.item = ItemExtension.ConstructItemWithParameters(
                id: new Guid(),
                url: string.Concat(onlineShop.Url, @"/firstItem"),
                title: "firstItem",
                description: "item description",
                price: new ItemPrice(20.5),
                onlineShop: onlineShop,
                type: ItemType.ComputerItem);

            //var a = Mock.Of<Func<MessageDialogViewModel, bool?>>();
            //var command = (RelayCommand<object>)this.itemInformationViewModel.UpdatePriceCommand;
        }

        [TearDown]
        public void TearDown()
        {
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void LoadItemPriceInformation_WillLoadData_Correctly()
        {
            var itemPricesCollection = new List<ItemPrice>()
            {
                new(15),
                new(16),
                new(20.5),
            };


            this.itemInformationViewModel = new ItemInformationViewModel(this.itemServiceMock.Object, this.item);

            Assert.AreEqual(itemPricesCollection.Count, this.itemInformationViewModel.LineSeries.Values.Count);
        }

        [Test]
        public void LoadItemPriceInformation_WhenExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UiEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemServiceMock.Object, this.item);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual($"Could not load price information for {this.item.Title}", messageDialogViewModel.Description);
        }

        [Test]
        public void UpdatePriceAction_WillGetNewPrice_UpdatesChart()
        {
            var updatedItemPrice = new ItemPrice(21);

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .ReturnsAsync(updatedItemPrice);

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemServiceMock.Object, this.item);

            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.AreEqual(1, this.itemInformationViewModel.Labels.Count);
            Assert.AreEqual(1, this.itemInformationViewModel.LineSeries.Values.Count);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public void UpdatePriceAction_WithNoNewPrice_WillShowMessageDialogAsync()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UiEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .ReturnsAsync(It.IsAny<ItemPrice>());

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemServiceMock.Object, this.item);

            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual("The price of the item hasn't been changed.", messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }

        [Test]
        public void UpdatePriceAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UiEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };
            var expectedOnExceptionDialogMessage = $"Could not update price for {this.item.Title}";

            this.itemServiceMock.Setup(_ => _.UpdateItemPrice(this.item))
                .Throws(new Exception());

            this.itemInformationViewModel = new ItemInformationViewModel(this.itemServiceMock.Object, this.item);

            this.itemInformationViewModel.UpdatePriceCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
            Assert.IsTrue(this.itemInformationViewModel.IsInProgress);
        }
    }
}
