using System;
using System.Threading.Tasks;

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

        private CreateItemViewModel createItemViewModel;

        [SetUp]
        public void SetUp()
        {
            this.itemServiceMock = new Mock<IItemService>();
        }

        [TearDown]
        public void TearDown()
        {
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public async Task AddItemAction_WillCreate_NewItem()
        {
            var onlineShop = OnlineShopExtension.ConstructOnlineShop(
                id: 1,
                url: "https://www.someShop.com",
                title: "someShop");
            var itemUrl = string.Concat(onlineShop.URL, @"/newItem");
            var itemType = Services.Models.ItemType.ComputerItem;

            this.createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemURL = itemUrl,
                SelectedItemType = itemType
            };

            this.itemServiceMock.Setup(_ => _.CreateItem(itemUrl, onlineShop, itemType));

            await this.createItemViewModel.AddItemCommand.ExecuteAsync();
        }

        [Test]
        public async Task AddItemAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            var onlineShop = OnlineShopExtension.ConstructOnlineShop(
                id: 1,
                url: "https://www.someShop.com",
                title: "someShop");
            var itemUrl = string.Concat(onlineShop.URL, @"/newItem");
            var itemType = Services.Models.ItemType.ComputerItem;

            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var exceptionMessage = "New exception thrown";

            this.createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemURL = itemUrl,
                SelectedItemType = itemType
            };

            this.itemServiceMock.Setup(_ => _.CreateItem(It.IsAny<string>(), onlineShop, It.IsAny<Services.Models.ItemType>()))
                .Throws(new Exception(exceptionMessage));

            await this.createItemViewModel.AddItemCommand.ExecuteAsync();

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(exceptionMessage, messageDialogViewModel.Description);
        }
    }
}
