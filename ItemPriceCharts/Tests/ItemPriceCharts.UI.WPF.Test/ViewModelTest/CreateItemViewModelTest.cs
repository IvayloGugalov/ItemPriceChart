﻿using System;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
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
            var itemUrl = string.Concat(onlineShop.Url, @"/newItem");
            var itemType = ItemType.ComputerItem;

            this.itemServiceMock.Setup(_ => _.AddItemToShop(itemUrl, onlineShop, itemType));

            var createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemUrl = itemUrl,
                SelectedItemType = itemType
            };

            createItemViewModel.AddItemCommand.Execute(null);
        }

        [Test]
        public void AddItemAction_WhenExceptionThrown_WillShowMessageDialog()
        {
            var onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();
            var itemUrl = string.Concat(onlineShop.Url, @"/newItem");
            var itemType = ItemType.ComputerItem;
            var expectedOnExceptionDialogMessage = $"Failed to create new item with url: {itemUrl}";

            MessageDialogViewModel messageDialogViewModel = null;
            UiEvents.ShowMessageDialog = viewmodel => { messageDialogViewModel = viewmodel; return false; };

            this.itemServiceMock.Setup(_ => _.AddItemToShop(It.IsAny<string>(), onlineShop, It.IsAny<ItemType>()))
                .Throws(new Exception());

            var createItemViewModel = new CreateItemViewModel(this.itemServiceMock.Object, onlineShop)
            {
                NewItemUrl = itemUrl,
                SelectedItemType = itemType
            };

            createItemViewModel.AddItemCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }
    }
}
