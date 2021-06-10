using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
    public class MainWindowViewModelTest
    {
        private Mock<IOnlineShopService> onlineShopServiceMock;
        private Mock<IItemService> itemServiceMock;

        private ItemListingViewModel itemListingViewModel;
        private ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private OnlineShop onlineShop;
        private UserAccount userAccount;

        [SetUp]
        public void SetUp()
        {
            this.onlineShopServiceMock = new Mock<IOnlineShopService>(MockBehavior.Strict);
            this.itemServiceMock = new Mock<IItemService>(MockBehavior.Strict);

            this.onlineShop = OnlineShopExtension.ConstructDefaultOnlineShop();

            this.userAccount = new UserAccount(
                firstName: "Firstname",
                lastName: "Lastname",
                email: new Email("newEmail@email.bg"),
                userName: "UserName",
                password: "P@ssWorD",
                onlineShops: new List<OnlineShop>());

            this.itemListingViewModel = new ItemListingViewModel(this.userAccount);
            this.shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.userAccount);
        }

        [TearDown]
        public void TearDown()
        {
            this.onlineShopServiceMock.VerifyAll();
            this.itemServiceMock.VerifyAll();
        }

        [Test]
        public void ConstructMainWindowViewModel_WhenSetOnlineShopsReturnsNull_WillShowMessageDialog()
        {
            //MessageDialogViewModel messageDialogViewModel = null;
            //UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            //var expectedOnExceptionDialogMessage = "Couldn't retrieve shops.";

            //var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            //Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }

        [Test]
        public void ConstructMainWindowViewModel_WhenSetOnlineShops_WillRetrieveShopsSuccessfully()
        {

            this.userAccount.AddOnlineShop(this.onlineShop);
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            Assert.AreEqual(1, mainWindowViewModel.OnlineShops.Count);
            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.First());
        }

        [Test]
        public void ShowItemListingAction_WithNoSelectedShop_WillSetItemsList()
        {
            //this.userAccount.AddOnlineShop(this.onlineShop);
            //var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            //mainWindowViewModel.ShowItemListingCommand.Execute(null);

            //Assert.IsNull(mainWindowViewModel.SelectedShop);
            //Assert.AreEqual(mainWindowViewModel.CurrentView, this.itemListingViewModel);
        }

        [Test]
        public void ShowItemListingAction_OnException_WillShowMessageDialog()
        {
            //MessageDialogViewModel messageDialogViewModel = null;
            //UIEvents.ShowMessageDialog = (viewmodel) => { messageDialogViewModel = viewmodel; return false; };

            //var expectedOnExceptionDialogMessage = "Couldn't retrieve items.";
            //var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            //mainWindowViewModel.ShowItemListingCommand.Execute(null);

            //Assert.AreEqual(expectedOnExceptionDialogMessage, messageDialogViewModel.Description);
        }

        [Test]
        public void ShowShopsAndItemListingsCommand_WillChangeCurrentView_Successfully()
        {
            //var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            //mainWindowViewModel.ShowShopsAndItemListingsCommand.Execute(null);

            //Assert.AreEqual(this.shopsAndItemListingsViewModel, mainWindowViewModel.CurrentView);
            //Assert.IsTrue(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test]
        public void ClearViewCommand_WillClearTheView_Successfully()
        {
            var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            mainWindowViewModel.ClearViewCommand.Execute(null);

            Assert.AreEqual(mainWindowViewModel, mainWindowViewModel.CurrentView);
            Assert.IsFalse(mainWindowViewModel.IsNewViewDisplayed);
        }

        [Test, Ignore("Event is raised 3 times when test is executed with all other tests")]
        public void OnAddedShop_WillUpdateListOfShops_Successfully()
        {

            var shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.userAccount);


            var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            Assert.AreEqual(Array.Empty<OnlineShop>(), mainWindowViewModel.OnlineShops);

            UiEvents.ShopAdded.Raise(this.onlineShop);

            Assert.AreEqual(this.onlineShop, mainWindowViewModel.OnlineShops.First());
        }

        [Test, Ignore("Event is raised 3 times when test is executed with all other tests")]
        public void OnDeletedShop_WillUpdateListOfShops_Successfully()
        {
            var listOfShops = new List<OnlineShop>() { this.onlineShop };


            var shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(this.userAccount);

    

            var mainWindowViewModel = new MainWindowViewModel(this.userAccount);

            Assert.AreEqual(listOfShops, mainWindowViewModel.OnlineShops);

            UiEvents.ShopDeleted.Raise(this.onlineShop);

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
