using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    [TestFixture]
    public class MainWindowViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private Mock<IItemService> itemServiceMock;

        private readonly TestableDispatcherWrapper dispatcherWrapper = new();
        private UiEvents uiEvents;

        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            Bootstrapper.Bootstrapper.Start(this.dispatcherWrapper);

            this.uiEvents = new UiEvents(this.dispatcherWrapper);

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
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void ClearViewCommand_WillClearTheView_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            mainWindowViewModel.SideMenuViewModel.ShowItemListingCommand.Execute(null);

            Assert.AreNotEqual(mainWindowViewModel, mainWindowViewModel.CurrentView);

            mainWindowViewModel.SideMenuViewModel.ClearViewCommand.Execute(null);

            Assert.AreEqual(mainWindowViewModel, mainWindowViewModel.CurrentView);
            Assert.IsFalse(mainWindowViewModel.IsNewViewDisplayed);
        }

        //private static IEnumerable<TestCaseData> ListOfItems
        //{
        //    get
        //    {
        //        yield return new TestCaseData(Array.Empty<Item>());
        //        yield return new TestCaseData(new List<Item>() { ItemExtension.ConstructItem(OnlineShopExtension.ConstructDefaultOnlineShop()) });
        //    }
        //}
    }
}
