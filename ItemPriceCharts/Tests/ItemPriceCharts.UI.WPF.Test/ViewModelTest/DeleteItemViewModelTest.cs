using System;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

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
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

            this.item = ItemExtension.ConstructItemWithParameters(
                id: new Guid(),
                url: string.Concat(onlineShop.Url, @"/firstItem"),
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
            UiEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

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
