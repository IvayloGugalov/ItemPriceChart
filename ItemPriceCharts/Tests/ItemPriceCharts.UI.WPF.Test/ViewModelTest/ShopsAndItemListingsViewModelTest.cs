﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class ShopsAndItemListingsViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private Mock<IItemService> itemServiceMock;

        private OnlineShop onlineShopWithItems;
        private OnlineShop onlineShopWithoutItems;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.onlineShopWithItems = OnlineShopExtension.ConstructDefaultOnlineShop();
            this.onlineShopWithItems.AddItem(ItemExtension.ConstructDefaultItem(this.onlineShopWithItems));

            this.onlineShopWithoutItems = OnlineShopExtension.ConstructOnlineShop(
                id: 2,
                url: "https://shop123.com",
                title: "shop123");
        }

        [TearDown]
        public void TearDown()
        {
            this.onlineShopServiceMock.VerifyAll();
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void AddShopsToViewModelAsync_WillRetrieveShops_Successfully()
        {
            var listOfShops = new List<OnlineShop>() { this.onlineShopWithItems, this.onlineShopWithoutItems };

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(listOfShops);

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object);

            Assert.AreEqual(listOfShops, shopsAndItemListingViewModel.OnlineShops);
            Assert.IsTrue(shopsAndItemListingViewModel.IsListOfShopsShown);
        }

        [Test]
        public void AddShopsToViewModelAsync_OnExceptionThrown_WillBeHandled()
        {
            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ThrowsAsync(new Exception());

            Assert.DoesNotThrow(() => new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object));
        }

        [Test]
        public void ShowItemsCommand_WillRetrive_AndShowItems()
        {
            var listOfShops = new List<OnlineShop>() { this.onlineShopWithItems };

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(listOfShops);

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object)
            {
                SelectedShop = this.onlineShopWithItems
            };

            shopsAndItemListingViewModel.ShowItemsCommand.Execute(null);

            Assert.AreEqual(this.onlineShopWithItems.Items.First(), shopsAndItemListingViewModel.ItemsList.First());
            Assert.IsTrue(shopsAndItemListingViewModel.AreItemsShown);
        }

        [Test]
        public void ShowItemsCommand_WhenExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var listOfShops = new List<OnlineShop>() { this.onlineShopWithItems };

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(listOfShops);

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object)
            {
                SelectedShop = null
            };

            shopsAndItemListingViewModel.ShowItemsCommand.Execute(null);

            var expectedErrorMessage = "Object reference not set to an instance of an object.";
            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedErrorMessage, messageDialogViewModel.Description);
            Assert.IsNull(shopsAndItemListingViewModel.ItemsList);
        }

        [Test]
        public void ShowItemsCommand_WithNoOnlineShops_WillBeDisabled()
        {
            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(new List<OnlineShop>());

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object);

            Assert.IsFalse(shopsAndItemListingViewModel.ShowItemsCommand.CanExecute(null));
        }

    }
}
