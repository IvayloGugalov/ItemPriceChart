using System;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Test.Extensions;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class CreateShopViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private OnlineShop onlineShop;
        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);

            this.onlineShop = OnlineShopExtension.ConstructOnlineShopWithParameters(
                id: new Guid(),
                url: "https://www.someShop.com",
                title: "someShop");

            this.userAccount = new UserAccount(
                firstName: "Firstname",
                lastName: "Lastname",
                email: new Email("newEmail@email.bg"),
                userName: "UserName",
                password: "P@ssWorD",
                onlineShops: new List<OnlineShop>());
        }

        [TearDown]
        public void TearDown()
        {
            this.onlineShopServiceMock.VerifyAll();
        }

        [Test]
        public void AddShopAction_WillCreateItem_Succeeds()
        {
            this.onlineShopServiceMock.Setup(_ => _.CreateShop(null, this.onlineShop.Url, this.onlineShop.Title));

            var createShopViewModel = new CreateShopViewModel(this.onlineShopServiceMock.Object, null)
            {
                NewShopUrl = this.onlineShop.Url,
                NewShopTitle = this.onlineShop.Title
            };

            createShopViewModel.AddShopCommand.Execute(null);
        }

        [Test]
        public void AddShopAction_OnExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UiEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var expectedOnExceptionDialogMessage = $"Failed to create new shop with url: {this.onlineShop.Url}";

            this.onlineShopServiceMock.Setup(_ => _.CreateShop(null, this.onlineShop.Url, this.onlineShop.Title))
                .Throws(new Exception());

            var createShopViewModel = new CreateShopViewModel(this.onlineShopServiceMock.Object, null)
            {
                NewShopUrl = this.onlineShop.Url,
                NewShopTitle = this.onlineShop.Title
            };

            createShopViewModel.AddShopCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }
    }
}
