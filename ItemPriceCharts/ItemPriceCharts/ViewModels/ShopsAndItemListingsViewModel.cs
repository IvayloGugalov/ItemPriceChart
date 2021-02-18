using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopsAndItemListingsViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ShopsAndItemListingsViewModel));

        private readonly IOnlineShopService onlineShopService;
        private bool isListOfShopsShown;

        public bool IsListOfShopsShown
        {
            get => this.isListOfShopsShown;
            set => this.SetValue(ref this.isListOfShopsShown, value);
        }

        public ObservableCollection<OnlineShop> OnlineShops { get; set; }

        public ICommand ShowItemsCommand { get; }
        public ICommand ShowCreateShopCommand { get; }
        public ICommand ShowAddItemCommand { get; }

        public ShopsAndItemListingsViewModel(ItemService itemService, OnlineShopService onlineShopService)
            : base (itemService)
        {
            this.onlineShopService = onlineShopService;

            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);

            this.ShouldShowShopInformation = true;

            UIEvents.ShopAdded.Subscribe(this.OnAddedShop);
            UIEvents.ShopDeleted.Subscribe(this.OnDeletedShop);

            this.AddShopsToViewModel();
        }

        private void ShowCreateShopAction() => UIEvents.ShowCreateShopView.Publish(null);
        private void ShowAddItemAction() => UIEvents.ShowCreateItemView.Publish(this.SelectedShop);

        private void AddItemsForShopAction(object parameter)
        {
            try
            {
                //Don't make call if the items for the shop are already shown
                if (this.ItemsList != null && this.ItemsList.Count > 0 &&
                    this.ItemsList.FirstOrDefault().OnlineShop.Equals(this.SelectedShop))
                {
                    return;
                }

                this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(
                    this.SelectedShop.Items);

                if (this.ItemsList.Any())
                {
                    this.AreItemsShown = true;
                }
            }
            catch (System.Exception e)
            {
                logger.Info($"Can't load items for {this.SelectedShop}: {e}");
                UIEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        "Error",
                        e.Message,
                        ButtonType.Close));
            }
        }

        private void OnAddedShop(object sender, OnlineShop e)
        {
            this.OnlineShops.Add(e);
            this.IsListOfShopsShown = true;
        }

        private void OnDeletedShop(object sender, OnlineShop e)
        {
            this.OnlineShops.Remove(e);
            this.OnPropertyChanged(() => this.IsListOfShopsShown);
        }

        private void AddShopsToViewModel()
        {
            try
            {
                this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(
                    this.onlineShopService.GetAllShops());

                this.IsListOfShopsShown = this.OnlineShops.Any();
            }
            catch (System.Exception e)
            {
                logger.Info($"Can't load shops: {e}");
            }
        }

        private bool ShowItemsPredicate()
        {
            return this.OnlineShops.Any();
        }
    }
}
