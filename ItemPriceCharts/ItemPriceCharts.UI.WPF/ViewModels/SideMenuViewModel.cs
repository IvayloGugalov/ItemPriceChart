using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Extensions;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Services;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class SideMenuViewModel : BaseViewModel
    {
        public ObservableCollection<OnlineShop> OnlineShops { get; }

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }
        private OnlineShop selectedShop;

        public bool IsLogOutModalOpen
        {
            get => this.isLogOutModalOpen;
            set => SetValue(ref this.isLogOutModalOpen, value);
        }
        private bool isLogOutModalOpen;

        public UserAccount UserAccount { get; }

        public ICommand ShowItemsForShopCommand { get; }
        public ICommand ShowItemListingCommand { get; }
        public ICommand ClearViewCommand { get; }
        public ICommand ShowLogFileCommand { get; }
        public ICommand ShowCreateShopCommand { get; }
        public ICommand ShowAddItemCommand { get; }

        public ICommand ShowUserSettingsCommand { get; }
        public ICommand ShowLogOutModalCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand CancelLogOutCommand { get; }

        public event Action<OnlineShop> ShowItems;
        public event Action ClearView;
        public event Action ShowUserSettings;

        public SideMenuViewModel(ILogOutService logOutService, UiEvents uiEvents, UserAccount userAccount)
        {
            this.UserAccount = userAccount;
            this.OnlineShops = userAccount.OnlineShopsForUser.Select(x => x.OnlineShop).ToObservableCollection();

            this.ShowItemsForShopCommand = new RelayCommand(_ => this.ShowItems?.Invoke(this.SelectedShop));
            this.ShowItemListingCommand = new RelayCommand(_ => this.ShowItems?.Invoke(null));
            this.ClearViewCommand = new RelayCommand(_ =>
            {
                this.ClearView?.Invoke();
                this.SelectedShop = null;
            });
            this.ShowLogFileCommand = new RelayCommand(_ => LogHelper.OpenLogFolder());
            this.ShowCreateShopCommand = new RelayCommand(_ => UiEvents.ShowCreateShopView.Raise(userAccount));
            this.ShowAddItemCommand = new RelayCommand(_ => UiEvents.ShowCreateItemView.Raise(this.SelectedShop));

            this.ShowUserSettingsCommand = new RelayCommand(_ => this.ShowUserSettings?.Invoke());
            this.ShowLogOutModalCommand = new RelayCommand(_ => this.IsLogOutModalOpen = true);
            this.LogOutCommand = new RelayCommand(_ => logOutService.LogOut());
            this.CancelLogOutCommand = new RelayCommand(_ => this.IsLogOutModalOpen = false);

            uiEvents.ShopAdded.Register(this.OnAddedShop);
            uiEvents.ShopDeleted.Register(this.OnDeletedShop);
        }

        private void OnAddedShop(OnlineShop e)
        {
            this.OnlineShops.Add(e);
        }

        private void OnDeletedShop(OnlineShop e)
        {
            this.OnlineShops.Remove(e);
        }
    }
}
