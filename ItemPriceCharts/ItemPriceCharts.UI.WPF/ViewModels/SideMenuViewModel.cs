using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Extensions;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class SideMenuViewModel : BaseViewModel
    {
        private OnlineShop selectedShop;
        private bool isOpen;

        public ObservableCollection<OnlineShop> OnlineShops { get; }

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }
        
        public bool IsOpen
        {
            get => this.isOpen;
            set => SetValue(ref this.isOpen, value);
        }

        public ICommand ShowAddItemCommand { get; }
        public ICommand ShowItemsForShopCommand { get; }
        public ICommand ClearViewCommand { get; }
        public ICommand ShowItemListingCommand { get; }
        public ICommand ShowLogFileCommand { get; }

        public ICommand ShowLogOutModalCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand CancelLogOutCommand { get; }

        public event Action<OnlineShop> ShowItems;
        public event Action ClearView;

        public SideMenuViewModel(UserAccount userAccount, UiEvents uiEvents)
        {
            this.OnlineShops = userAccount.OnlineShopsForUser.Select(x => x.OnlineShop).ToObservableCollection();

            this.ShowItemListingCommand = new RelayCommand(_ => this.ShowItems?.Invoke(null));
            this.ShowItemsForShopCommand = new RelayCommand(_ => this.ShowItems?.Invoke(this.SelectedShop));
            this.ClearViewCommand = new RelayCommand(_ =>
            {
                this.ClearView?.Invoke();
                this.SelectedShop = null;
            });
            this.ShowLogFileCommand = new RelayCommand(_ => LogHelper.OpenLogFolder());
            this.ShowAddItemCommand = new RelayCommand(_ => UiEvents.ShowCreateItemView.Raise(this.SelectedShop));

            this.ShowLogOutModalCommand = new RelayCommand(_ => this.IsOpen = true);
            this.LogOutCommand = new RelayCommand(_ => { });
            this.CancelLogOutCommand = new RelayCommand(_ => this.IsOpen = false);

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
