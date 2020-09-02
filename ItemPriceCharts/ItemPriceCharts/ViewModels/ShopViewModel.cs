using System.Collections.ObjectModel;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopViewModel : BindableViewModel
    {
        private ObservableCollection<ItemModel> itemsList;
        private OnlineShopModel selectedShop;
        private ItemModel selectedItem;
        private bool isAddShopShown;
        private bool isChartShown;
        private bool areItemsShown;

        public ObservableCollection<ItemModel> ItemsList
        {
            get => this.itemsList;
            set => this.SetValue(ref this.itemsList, value);
        }

        public ObservableCollection<OnlineShopModel> OnlineShops { get; set; }

        public OnlineShopModel SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public ItemModel SelectedItem
        {
            get => this.selectedItem;
            set => SetValue(ref this.selectedItem, value);
        }

        public bool IsAddShopShown
        {
            get => this.isAddShopShown;
            set => this.SetValue(ref this.isAddShopShown, value);
        }

        public bool IsChartShown
        {
            get => this.isChartShown;
            set => this.SetValue(ref this.isChartShown, value);
        }

        public bool AreItemsShown
        {
            get => this.areItemsShown;
            set => this.SetValue(ref this.areItemsShown, value);
        }

        public ICommand ShowCreateShopCommand { get; }

        public ICommand ShowDeleteShopCommand { get; }

        public ICommand ShowAddItemCommand { get;  }

        public ShopViewModel()
        {
            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowDeleteShopCommand = new RelayCommand(_ => this.ShowDeleteShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
        }

        private void ShowCreateShopAction() => UIEvents.ShowCreateShopViewModel.Publish(null);

        private void ShowDeleteShopAction() => UIEvents.ShowDeleteShopViewModel.Publish(null);

        private void ShowAddItemAction() => UIEvents.ShowCreateItemViewModel.Publish(this.SelectedShop);
    }
}
