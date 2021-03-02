using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class ItemListingViewModelTest
    {
        private Mock<IItemService> itemServiceMock;
        private OnlineShop defaultOnlineShop;

        [SetUp]
        public void SetUp()
        {
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.defaultOnlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();
        }

        [TearDown]
        public void TearDown()
        {
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public async Task GetItems_WithNoSelectedShop_WillCallService()
        {
            var items = new List<Item>()
            {
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop),
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop),
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop)
            };

            this.itemServiceMock.Setup(_ => _.GetAllItems())
                .ReturnsAsync(items);

            var itemListingViewModel = new ItemListingViewModel(this.itemServiceMock.Object)
            {
                SelectedShop = null
            };

            await itemListingViewModel.SetItemsListAsync();

            Assert.AreEqual(items.Count, itemListingViewModel.ItemsList.Count);
            Assert.AreEqual(items.First(), itemListingViewModel.SelectedItem);
        }

        [Test]
        public async Task GetItems_WithSelectedShop_WillRetrieveItems()
        {
            var items = new List<Item>()
            {
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop),
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop),
                ItemExtension.ConstructDefaultItem(this.defaultOnlineShop)
            };

            foreach (var item in items)
            {
                this.defaultOnlineShop.AddItem(item);
            }

            var itemListingViewModel = new ItemListingViewModel(this.itemServiceMock.Object)
            {
                SelectedShop = this.defaultOnlineShop
            };

            await itemListingViewModel.SetItemsListAsync();

            Assert.AreEqual(items.Count, itemListingViewModel.ItemsList.Count);
            Assert.AreEqual(items.First(), itemListingViewModel.SelectedItem);
        }
    }
}
