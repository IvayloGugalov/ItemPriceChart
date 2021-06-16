using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class ItemListingViewModelTest
    {
        private OnlineShop defaultOnlineShop;
        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.defaultOnlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

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
        }

        [Test]
        public void GetItems_WithNoSelectedShop_WillCallService()
        {
            var items = new List<Item>()
            {
                ItemExtension.ConstructItem(this.defaultOnlineShop),
                ItemExtension.ConstructItem(this.defaultOnlineShop),
                ItemExtension.ConstructItem(this.defaultOnlineShop)
            };

            foreach (var item in items)
            {
                this.defaultOnlineShop.AddItem(item);
            }

            this.userAccount.AddOnlineShop(this.defaultOnlineShop);

            var itemListingViewModel = new ItemListingViewModel(this.userAccount)
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
            var items = new List<Item>()
            {
                ItemExtension.ConstructItem(this.defaultOnlineShop),
                ItemExtension.ConstructItem(this.defaultOnlineShop),
                ItemExtension.ConstructItem(this.defaultOnlineShop)
            };

            foreach (var item in items)
            {
                this.defaultOnlineShop.AddItem(item);
            }

            var itemListingViewModel = new ItemListingViewModel(this.userAccount)
            {
                SelectedShop = this.defaultOnlineShop
            };

            itemListingViewModel.SetItemsList();

            Assert.AreEqual(items.Count, itemListingViewModel.ItemsList.Count);
            Assert.AreEqual(items.First(), itemListingViewModel.SelectedItem);
        }
    }
}
