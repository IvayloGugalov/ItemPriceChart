using System.Collections.ObjectModel;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopViewModel : BindableViewModel
    {
        private readonly WebPageService webPageService;

        private OnlineShopModel selectedShop;
        private ItemViewModel selectedItem;
        private bool isAddShopShown;
        private bool isChartShown;
        private bool areItemsShown;

        public ObservableCollection<ItemViewModel> ItemsList { get; set; }
        public ObservableCollection<OnlineShopModel> OnlineShops { get; set; }

        public OnlineShopModel SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public ItemViewModel SelectedItem
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

        public ShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowDeleteShopCommand = new RelayCommand(_ => this.ShowDeleteShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
        }

        private void ShowCreateShopAction()
        {
            _ = new CreateShopViewModel(this.webPageService);
        }

        private void ShowDeleteShopAction()
        {
            _ = new DeleteShopViewModel(this.webPageService);
        }

        private void ShowAddItemAction()
        {
            _ = new CreateItemViewModel(this.webPageService, this.SelectedShop);
        }
    }
}
