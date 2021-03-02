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
    public class MainWindowViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private Mock<IItemService> itemServiceMock;

        private ItemListingViewModel itemListingViewModel;
        private ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private OnlineShop onlineShop;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(Array.Empty<OnlineShop>());

            this.itemListingViewModel = new ItemListingViewModel(this.itemServiceMock.Object);
            this.shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object);

            this.onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

        }

        [TearDown]
        public void TearDown()
        {
            this.onlineShopServiceMock.VerifyAll();
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void ConstructMainWindowViewModel_WhenSetOnlineShopsReturnsNull_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var expectedOnExceptionDialogMessage = "Couldn't retrieve shops.";

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(It.IsAny<IEnumerable<OnlineShop>>());

            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }

        [Test]
        public void ConstructMainWindowViewModel_WhenSetOnlineShops_WillRetrieveShopsSuccessfully()
        {
            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(new List<OnlineShop>()
                {
                    this.onlineShop
                });

            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            Assert.AreEqual(1, mainWindowViewModel.OnlineShops.Count);
            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.First());
        }

        [Test]
        public void ShowItemListingAction_WithNoSelectedShop_WillSetItemsList()
        {
            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(Array.Empty<OnlineShop>());

            this.itemServiceMock.Setup(_ => _.GetAllItems())
                .ReturnsAsync(new List<Item> { ItemExtension.ConstructDefaultItem(this.onlineShop) });

            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            mainWindowViewModel.ShowItemListingCommand.Execute(null);

            Assert.IsNull(mainWindowViewModel.SelectedShop);
            Assert.AreEqual(mainWindowViewModel.CurrentView, this.itemListingViewModel);
        }

        [Test]
        public void ShowItemListingAction_OnException_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var expectedOnExceptionDialogMessage = "Couldn't retrieve items.";

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(Array.Empty<OnlineShop>());

            this.itemServiceMock.Setup(_ => _.GetAllItems())
                .Throws(new Exception());

            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            mainWindowViewModel.ShowItemListingCommand.Execute(null);

            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }

        [Test]
        public void ShowShopsAndItemListingsCommand_WillChangeCurrentView_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            mainWindowViewModel.ShowShopsAndItemListingsCommand.Execute(null);

            Assert.AreEqual(this.shopsAndItemListingsViewModel, mainWindowViewModel.CurrentView);
            Assert.IsTrue(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test]
        public void ClearViewCommand_WillClearTheView_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            mainWindowViewModel.ClearViewCommand.Execute(null);

            Assert.AreEqual(mainWindowViewModel, mainWindowViewModel.CurrentView);
            Assert.IsFalse(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test, Ignore("Event is raised 3 times when test is executed with all other tests")]
        public void OnAddedShop_WillUpdateListOfShops_Successfully()
        {
            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(Array.Empty<OnlineShop>());

            var shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object);

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(Array.Empty<OnlineShop>());

            var mainWindowViewModel = new MainWindowViewModel(shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            Assert.AreEqual(Array.Empty<OnlineShop>(), mainWindowViewModel.OnlineShops);

            UIEvents.ShopAdded.Publish(this.onlineShop);

            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.First());
        }

        [Test, Ignore("Event is raised 3 times when test is executed with all other tests")]
        public void OnDeletedShop_WillUpdateListOfShops_Successfully()
        {
            var listOfShops = new List<OnlineShop>() { this.onlineShop };

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(listOfShops);

            var shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.itemServiceMock.Object, this.onlineShopServiceMock.Object);

            this.onlineShopServiceMock.Setup(_ => _.GetAllShops())
                .ReturnsAsync(listOfShops);

            var mainWindowViewModel = new MainWindowViewModel(shopsAndItemListingsViewModel, this.itemListingViewModel, this.onlineShopServiceMock.Object);

            Assert.AreEqual(listOfShops, mainWindowViewModel.OnlineShops);

            UIEvents.ShopDeleted.Publish(this.onlineShop);

            Assert.AreEqual(Array.Empty<OnlineShop>(), mainWindowViewModel.OnlineShops);
        }


        private static IEnumerable<TestCaseData> ListOfItems
        {
            get
            {
                yield return new TestCaseData(Array.Empty<Item>());
                yield return new TestCaseData(new List<Item>() { ItemExtension.ConstructDefaultItem(OnlineShopExtension.ConstructDefaultOnlineShop()) });
            }
        }
    }
}
