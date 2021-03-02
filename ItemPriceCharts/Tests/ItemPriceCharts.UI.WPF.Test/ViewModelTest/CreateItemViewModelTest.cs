using System;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class CreateItemViewModelTest
    {
        private Mock<IItemService> itemServiceMock;

        [SetUp]
        public void SetUp()
        {
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void AddItemAction_WillCreate_NewItem()
        {
            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();
            var itemUrl = string.Concat(onlineShop.URL, @"/newItem");
            var itemType = Services.Models.ItemType.ComputerItem;

            this.itemServiceMock.Setup(_ => _.CreateItem(itemUrl, onlineShop, itemType));

            var createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemURL = itemUrl,
                SelectedItemType = itemType
            };

            createItemViewModel.AddItemCommand.Execute(null);
        }

        [Test]
        public void AddItemAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();
            var itemUrl = string.Concat(onlineShop.URL, @"/newItem");
            var itemType = Services.Models.ItemType.ComputerItem;
            var expectedOnExceptionDialogMessage = $"Failed to create new item with url: {itemUrl}";

            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            this.itemServiceMock.Setup(_ => _.CreateItem(It.IsAny<string>(), onlineShop, It.IsAny<Services.Models.ItemType>()))
                .Throws(new Exception());

            var createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemURL = itemUrl,
                SelectedItemType = itemType
            };

            createItemViewModel.AddItemCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }
    }
}
