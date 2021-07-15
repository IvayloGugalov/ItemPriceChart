using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Extensions;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopsAndItemListingsViewModel : BaseListingViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ShopsAndItemListingsViewModel));

        private bool isListOfShopsShown;

        public bool IsListOfShopsShown
        {
            get => this.isListOfShopsShown;
            private set => this.SetValue(ref this.isListOfShopsShown, value);
        }

        public ObservableCollection<OnlineShop> OnlineShops { get; set; }

        public ICommand ShowItemsCommand { get; }
        public ICommand ShowCreateShopCommand { get; }
        public ICommand ShowAddItemCommand { get; }

        public ShopsAndItemListingsViewModel(UserAccount userAccount, UiEvents uiEvents)
            : base (userAccount, uiEvents)
        {
            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);

            this.AddShopsToViewModel();
            this.ShouldShowShopInformation = true;

            uiEvents.ShopAdded.Register(this.OnAddedShop);
            uiEvents.ShopDeleted.Register(this.OnDeletedShop);
        }

        private void ShowCreateShopAction() => UiEvents.ShowCreateShopView.Raise(this.UserAccount);
        private void ShowAddItemAction() => UiEvents.ShowCreateItemView.Raise(this.SelectedShop);

        private void AddItemsForShopAction(object parameter)
        {
            try
            {
                //Don't make call if the items for the shop are already shown
                if (this.ItemsList is {Count: > 0} &&
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
            catch (Exception e)
            {
                Logger.Info($"Can't load items for {this.SelectedShop}:\n{e}");
                UiEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        "Error",
                        e.Message,
                        ButtonType.Close));
            }
        }

        private void OnAddedShop(OnlineShop e)
        {
            this.OnlineShops.Add(e);
            this.IsListOfShopsShown = true;
        }

        private void OnDeletedShop(OnlineShop e)
        {
            this.OnlineShops.Remove(e);
            this.OnPropertyChanged(() => this.IsListOfShopsShown);
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
