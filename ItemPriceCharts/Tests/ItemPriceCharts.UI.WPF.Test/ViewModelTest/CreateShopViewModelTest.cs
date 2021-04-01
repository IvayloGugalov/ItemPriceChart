using System;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
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

            this.onlineShop = OnlineShopExtension.ConstructOnlineShop(
                id: 1,
                url: "https://www.someShop.com",
                title: "someShop");

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
        }

        [Test]
        public void AddShopAction_WillCreateItem_Succeeds()
        {
            this.onlineShopServiceMock.Setup(_ => _.CreateShop(this.onlineShop.URL, this.onlineShop.Title, this.userAccount));

            var createShopViewModel = new CreateShopViewModel(this.onlineShopServiceMock.Object, this.userAccount)
            {
                NewShopURL = this.onlineShop.URL,
                NewShopTitle = this.onlineShop.Title
            };

            createShopViewModel.AddShopCommand.Execute(null);
        }

        [Test]
        public void AddShopAction_OnExceptionThrown_WillShowMessageDialog()
        {
            MessageDialogViewModel messageDialogViewModel = null;
            UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            var expectedOnExceptionDialogMessage = $"Failed to create new shop with url: {this.onlineShop.URL}";

            this.onlineShopServiceMock.Setup(_ => _.CreateShop(this.onlineShop.URL, this.onlineShop.Title, this.userAccount))
                .Throws(new Exception());

            var createShopViewModel = new CreateShopViewModel(this.onlineShopServiceMock.Object, this.userAccount)
            {
                NewShopURL = this.onlineShop.URL,
                NewShopTitle = this.onlineShop.Title
            };

            createShopViewModel.AddShopCommand.Execute(null);

            Assert.IsNotNull(messageDialogViewModel);
            Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }
    }
}
