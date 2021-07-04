using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class ItemListingViewModelTest
    {
        private readonly TestableDispatcherWrapper dispatcherWrapper = new();
        private UiEvents uiEvents;

        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.userAccount = new UserAccount(
                firstName: "Firstname",
                lastName: "Lastname",
                email: new Email("newEmail@email.bg"),
                userName: "UserName",
                password: "P@ssWorD");

            this.uiEvents = new UiEvents(this.dispatcherWrapper);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetItems_WithNoSelectedShop_WillCallService()
        {
            var shop = OnlineShopExtension.ConstructDefaultOnlineShop();
            var items = new List<Item>()
            {
                ItemExtension.ConstructItem(shop)
            };

            foreach (var item in items)
            {
                shop.AddItem(item);
            }

            this.userAccount.AddOnlineShop(shop);

            var itemListingViewModel = new ItemListingViewModel(this.userAccount, this.uiEvents)
            {
                SelectedShop = null
            };

            itemListingViewModel.SetItemsList();

            Assert.AreEqual(items.Count, itemListingViewModel.ItemsList.Count);
            Assert.AreEqual(items.First(), itemListingViewModel.SelectedItem);
        }

        [Test]
        public void GetItems_WithSelectedShop_WillRetrieveItems()
        {
            var shop = OnlineShopExtension.ConstructDefaultOnlineShop();
            var items = new List<Item>()
            {
                ItemExtension.ConstructItem(shop)
            };

            foreach (var item in items)
            {
                shop.AddItem(item);
            }

            var itemListingViewModel = new ItemListingViewModel(this.userAccount, this.uiEvents)
            {
                SelectedShop = shop
            };

            itemListingViewModel.SetItemsList();

            Assert.AreEqual(items.Count, itemListingViewModel.ItemsList.Count);
            Assert.AreEqual(items.First(), itemListingViewModel.SelectedItem);
        }
    }
}
