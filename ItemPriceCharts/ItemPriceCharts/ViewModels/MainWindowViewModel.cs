using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(MainWindowViewModel));

        private readonly ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private readonly ItemListingViewModel itemListingViewModel;
        private readonly IOnlineShopService onlineShopService;
        private object currentView;
        private bool isNewViewDisplayed;
        private OnlineShop selectedShop;

        public bool IsNewViewDisplayed
        {
            get => this.isNewViewDisplayed;
            set => this.SetValue(ref this.isNewViewDisplayed, value);
        }

        public object CurrentView
        {
            get => this.currentView;
            set => this.SetValue(ref this.currentView, value);
        }

        public ObservableCollection<OnlineShop> OnlineShops { get; private set; }

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public ICommand ShowShopsAndItemListingsCommand { get; }
        public ICommand ClearViewCommand { get; }
        public ICommand ShowLogFileCommand { get; }
        public IAsyncCommand ShowItemListingCommand { get; }
        public IAsyncCommand ShowItemsForShopCommand { get; }

        public MainWindowViewModel(ShopsAndItemListingsViewModel shopsAndItemListingsViewModel, ItemListingViewModel itemListingViewModel, IOnlineShopService onlineShopService)
        {
            this.shopsAndItemListingsViewModel = shopsAndItemListingsViewModel;
            this.itemListingViewModel = itemListingViewModel;
            this.onlineShopService = onlineShopService;
            this.currentView = this;

            this.ShowShopsAndItemListingsCommand = new RelayCommand(_ => this.ShowShopsAndItemListingsAction());
            this.ShowItemListingCommand = new RelayAsyncCommand(this.ShowItemListingAction, errorHandler: e =>
            this.ErrorHandler(exception: e, errorMessage: "Couldn't retrieve items."));
            this.ShowItemsForShopCommand = new RelayAsyncCommand(this.ShowItemListingAction, errorHandler: e =>
            this.ErrorHandler(exception: e, errorMessage: "Couldn't retrieve items."));
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
            this.ShowLogFileCommand = new RelayCommand(_ => LogHelper.OpenLogFolder());

            UIEvents.ShopAdded.Subscribe(this.OnAddedShop);
            UIEvents.ShopDeleted.Subscribe(this.OnDeletedShop);

            this.SetOnlineShopsAsync().FireAndForgetSafeAsync(errorHandler: e => this.ErrorHandler(exception: e, errorMessage: "Couldn't retrieve shops."));
        }

        private async Task SetOnlineShopsAsync()
        {
            var retrievedOnlineShops = await this.onlineShopService.GetAllShops();

            this.OnlineShops = retrievedOnlineShops.ToObservableCollection();
        }

        private void ErrorHandler(Exception exception, string errorMessage)
        {
            logger.Info($"Failed to retrieve entities.\t{exception.Message}");
            MessageDialogCreator.ShowErrorDialog(message: errorMessage);
        }

        private void OnAddedShop(object sender, OnlineShop e)
        {
            this.OnlineShops.Add(e);
        }

        private void OnDeletedShop(object sender, OnlineShop e)
        {
            this.OnlineShops.Remove(e);
        }

        private void ClearViewAction()
        {
            this.ResetSelectedShop();
            this.CurrentView = this;
            this.IsNewViewDisplayed = false;
        }

        private void ShowShopsAndItemListingsAction()
        {
            this.ResetSelectedShop();
            this.CurrentView = this.shopsAndItemListingsViewModel;
            this.IsNewViewDisplayed = true;
        }

        private async Task ShowItemListingAction()
        {
            this.itemListingViewModel.SelectedShop = this.SelectedShop;

            await this.itemListingViewModel.SetItemsListAsync();

            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ResetSelectedShop()
        {
            this.SelectedShop = null;
        }
    }
}
