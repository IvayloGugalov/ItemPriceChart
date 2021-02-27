using System;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class DeleteItemViewModelTest
    {
        private Mock<IItemService> itemServiceMock;
        private Item item;

        [SetUp]
        public void SetUp()
        {
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
        }

        [TearDown]
        public void TearDown()
        {
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void DeleteItemAction_WhenExecuted_WillDeleteItem()
        {
            this.itemServiceMock.Setup(_ => _.DeleteItem(this.item));

            var deleteItemViewModel = new DeleteItemViewModel(this.itemServiceMock.Object, this.item);

            deleteItemViewModel.DeleteItemCommand.Execute(null);
        }

        [Test]
        public void DeleteItemAction_OnExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var expectedOnExceptionDialogMessage = $"Couldn't delete item {this.item.Title}";

            this.itemServiceMock.Setup(_ => _.DeleteItem(this.item))
                .Throws(new Exception());

            var deleteItemViewModel = new DeleteItemViewModel(this.itemServiceMock.Object, this.item);

            deleteItemViewModel.DeleteItemCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }
    }
}
