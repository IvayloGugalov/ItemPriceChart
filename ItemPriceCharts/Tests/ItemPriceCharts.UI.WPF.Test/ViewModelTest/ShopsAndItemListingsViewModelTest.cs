using System;
using System.Collections.Generic;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class ShopsAndItemListingsViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private Mock<IItemService> itemServiceMock;

        private readonly TestableDispatcherWrapper dispatcherWrapper = new();
        private UiEvents uiEvents;

        private OnlineShop onlineShopWithItems;
        private OnlineShop onlineShopWithoutItems;
        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.uiEvents = new UiEvents(this.dispatcherWrapper);

            this.onlineShopWithItems = OnlineShopExtension.ConstructDefaultOnlineShop();
            this.onlineShopWithItems.AddItem(ItemExtension.ConstructItem(this.onlineShopWithItems));

            this.onlineShopWithoutItems = OnlineShopExtension.ConstructOnlineShopWithParameters(
                id: new Guid(),
                url: "https://shop123.com",
                title: "shop123");

            this.userAccount = new UserAccount(
                firstName: "Firstname",
                lastName: "Lastname",
                email: new Email("newEmail@email.bg"),
                userName: "UserName",
                password: "P@ssWorD");
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
            var listOfShops = new List<OnlineShop> { this.onlineShopWithItems, this.onlineShopWithoutItems };

            foreach (var shop in listOfShops)
            {
                this.userAccount.AddOnlineShop(shop);
            }

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents);

            Assert.AreEqual(listOfShops, shopsAndItemListingViewModel.OnlineShops);
            Assert.IsTrue(shopsAndItemListingViewModel.IsListOfShopsShown);
        }

        [Test]
        public void AddShopsToViewModelAsync_OnExceptionThrown_WillBeHandled()
        {
            ShopsAndItemListingsViewModel shopsAndItemListingViewModel = null;

            Assert.DoesNotThrow(() =>
            {
                shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents);
            });

            Assert.IsFalse(shopsAndItemListingViewModel.IsListOfShopsShown);
        }

        [Test]
        public void ShowItemsCommand_WillRetrieve_AndShowItems()
        {
            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents)
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
            UiEvents.ShowMessageDialog = viewmodel => { messageDialogViewModel = viewmodel; return false; };

            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents)
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
            var shopsAndItemListingViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents);

            Assert.IsFalse(shopsAndItemListingViewModel.ShowItemsCommand.CanExecute(null));
        }

    }
}
