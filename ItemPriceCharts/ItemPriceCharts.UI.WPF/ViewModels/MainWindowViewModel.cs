using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(MainWindowViewModel));

        private readonly ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private readonly ItemListingViewModel itemListingViewModel;

        private BindableViewModel currentView;
        private bool isNewViewDisplayed;
        private OnlineShop selectedShop;

        public UserAccount UserAccount { get; }

        public bool IsNewViewDisplayed
        {
            get => this.isNewViewDisplayed;
            set => this.SetValue(ref this.isNewViewDisplayed, value);
        }

        public BindableViewModel CurrentView
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
        public ICommand ShowItemListingCommand { get; }
        public ICommand ShowItemsForShopCommand { get; }
        public ICommand ClosedCommand => new RelayCommand(_ => UIEvents.CloseApplication());

        public MainWindowViewModel(UserAccount userAccount)
        {
            this.shopsAndItemListingsViewModel = new ShopsAndItemListingsViewModel(userAccount);
            this.itemListingViewModel = new ItemListingViewModel(userAccount);
            
            this.UserAccount = userAccount;
            this.currentView = this;

            //this.OnlineShops = this.UserAccount.OnlineShopsForUser.ToObservableCollection();

            this.ShowShopsAndItemListingsCommand = new RelayCommand(_ => this.ShowShopsAndItemListingsAction());
            this.ShowItemListingCommand = new RelayCommand(_ => this.ShowItemListingAction());
            this.ShowItemsForShopCommand = new RelayCommand(_ => this.ShowItemListingAction());
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
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
            this.CurrentView = this;
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

            this.itemListingViewModel.SetItemsList();

            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ResetSelectedShop()
        {
            this.SelectedShop = null;
        }
    }
}
