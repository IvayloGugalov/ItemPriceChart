using System.Collections.ObjectModel;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private readonly ItemListingViewModel itemListingViewModel;

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

        public ICommand ShowShopsAndItemListingsCommand { get; }
        public ICommand ShowItemListingCommand { get; }
        public ICommand ClearViewCommand { get; }
        public ICommand ShowLogFileCommand { get; }

        public ObservableCollection<OnlineShop> OnlineShops { get; private set; }

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public ICommand ShowItemsForShopCommand { get; }

        public MainWindowViewModel(ShopsAndItemListingsViewModel shopsAndItemListingsViewModel, ItemListingViewModel itemListingViewModel, OnlineShopService onlineShopService)
        {
            this.shopsAndItemListingsViewModel = shopsAndItemListingsViewModel;
            this.itemListingViewModel = itemListingViewModel;
            this.currentView = this;

            this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(onlineShopService.GetAllShops());

            this.ShowShopsAndItemListingsCommand = new RelayCommand(_ => this.ShowShopsAndItemListingsAction());
            this.ShowItemListingCommand = new RelayCommand(_ => this.ShowItemListingAction());
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
            this.ShowItemsForShopCommand = new RelayCommand(_ => this.ShowItemListingAction());
            this.ShowLogFileCommand = new RelayCommand(_ => LogHelper.OpenLogFolder());

            UIEvents.ShopAdded.Subscribe(this.OnAddedShop);
            UIEvents.ShopDeleted.Subscribe(this.OnDeletedShop);
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
            this.IsNewViewDisplayed = false;
        }

        private void ShowShopsAndItemListingsAction()
        {
            this.ResetSelectedShop();
            this.CurrentView = this.shopsAndItemListingsViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ShowItemListingAction()
        {
            this.itemListingViewModel.SelectedShop = this.SelectedShop;
            this.itemListingViewModel.ShowItems().FireAndForgetSafeAsync(shouldAwait: false);
            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ResetSelectedShop()
        {
            this.SelectedShop = null;
        }
    }
}
