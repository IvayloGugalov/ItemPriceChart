using System;
using System.Collections.Generic;
using System.Linq;

using Moq;
using NUnit.Framework;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Events;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Test.Extensions;
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

        private OnlineShop onlineShop;
        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.uiEvents = new UiEvents(this.dispatcherWrapper);

            this.onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

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
        public void ConstructMainWindowViewModel_WhenSetOnlineShops_WillRetrieveShopsSuccessfully()
        {

            this.userAccount.AddOnlineShop(this.onlineShop);
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            Assert.AreEqual(1, mainWindowViewModel.OnlineShops.Count);
            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.First());
        }

        [Test]
        public void ShowItemListingAction_WithNoSelectedShop_WillSetItemsList()
        {
            this.userAccount.AddOnlineShop(this.onlineShop);
            var itemListingViewModel = new ItemListingViewModel(this.userAccount, this.uiEvents);
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            mainWindowViewModel.ShowItemListingCommand.Execute(null);

            Assert.IsNull(mainWindowViewModel.SelectedShop);
            Assert.AreEqual(itemListingViewModel.GetType(), mainWindowViewModel.CurrentView.GetType());
        }

        [Test]
        public void ShowShopsAndItemListingsCommand_WillChangeCurrentView_Successfully()
        {
            var shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.userAccount, this.uiEvents);
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            mainWindowViewModel.ShowShopsAndItemListingsCommand.Execute(null);

            Assert.AreEqual(shopsAndItemListingsViewModel.GetType(), mainWindowViewModel.CurrentView.GetType());
            Assert.IsTrue(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test]
        public void ClearViewCommand_WillClearTheView_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            mainWindowViewModel.ClearViewCommand.Execute(null);

            Assert.AreEqual(mainWindowViewModel, mainWindowViewModel.CurrentView);
            Assert.IsFalse(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test]
        public void OnAddedShop_WillUpdateListOfShops_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            Assert.AreEqual(Array.Empty<OnlineShop>(), mainWindowViewModel.OnlineShops);

            DomainEvents.ShopAdded.Raise(this.onlineShop);

            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.FirstOrDefault());
        }

        [Test]
        public void OnDeletedShop_WillUpdateListOfShops_Successfully()
        {
            var listOfShops = new List<OnlineShop> { this.onlineShop };
            this.userAccount.AddOnlineShop(listOfShops.First());
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount, this.uiEvents);

            Assert.AreEqual(listOfShops, mainWindowViewModel.OnlineShops);

            DomainEvents.ShopDeleted.Raise(this.onlineShop);

            Assert.AreEqual(Array.Empty<OnlineShop>(), mainWindowViewModel.OnlineShops);
        }


        private static IEnumerable<TestCaseData> ListOfItems
        {
            get
            {
                yield return new TestCaseData(Array.Empty<Item>());
                yield return new TestCaseData(new List<Item>() { ItemExtension.ConstructItem(OnlineShopExtension.ConstructDefaultOnlineShop()) });
            }
        }
    }
}
