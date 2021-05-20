using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopsAndItemListingsViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ShopsAndItemListingsViewModel));

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

        public ShopsAndItemListingsViewModel(UserAccount userAccount)
            : base (userAccount)
        {
            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);

            this.AddShopsToViewModel();
            this.ShouldShowShopInformation = true;

            UIEvents.ShopAdded.Subscribe(this.OnAddedShop);
            UIEvents.ShopDeleted.Subscribe(this.OnDeletedShop);
        }

        private void ShowCreateShopAction() => UIEvents.ShowCreateShopView.Publish(this.UserAccount);
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

                this.ItemsList = this.SelectedShop.Items.ToObservableCollection();

                if (this.ItemsList.Any())
                {
                    this.AreItemsShown = true;
                }
            }
            catch (System.Exception e)
            {
                logger.Info($"Can't load items for {this.SelectedShop}:\n{e}");
                UIEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        "Error",
                        e.Message,
                        ButtonType.Close));
            }
        }

        private void OnAddedShop(object sender, OnlineShop e)
        {
            // Dali trqbva tuk da se vika add/remove?
            //this.OnlineShops.Add(e);
            //this.UserAccount.AddOnlineShop(e);
            //this.IsListOfShopsShown = true;
        }

        private void OnDeletedShop(object sender, OnlineShop e)
        {
            //this.OnlineShops.Remove(e);
            //this.UserAccount.RemoveOnlineShop(e);
            //this.OnPropertyChanged(() => this.IsListOfShopsShown);
        }

        private void AddShopsToViewModel()
        {
            this.OnlineShops = this.UserAccount.OnlineShopsForUser.Select(x => x.OnlineShop).ToObservableCollection();

            this.IsListOfShopsShown = this.OnlineShops.Any();
        }

        private bool ShowItemsPredicate()
        {
            return this.OnlineShops.Any();
        }
    }
}
