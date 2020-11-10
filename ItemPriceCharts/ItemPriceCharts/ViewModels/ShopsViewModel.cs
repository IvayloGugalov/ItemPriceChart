using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopsViewModel : ShopViewModel
    {
        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;
       
        public ICommand ShowItemsCommand { get; }
        public ICommand ShowCreateShopCommand { get; }
        public ICommand ShowDeleteShopCommand { get; }

        public ShopsViewModel(ItemService itemService, OnlineShopService onlineShopService)
            : base (itemService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;

            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowDeleteShopCommand = new RelayCommand(_ => this.ShowDeleteShopAction());
            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);

            this.ShouldShowShopInformation = true;

            EventsLocator.ShopAdded.Subscribe(this.AddShopToShopsList);
            EventsLocator.ShopDeleted.Subscribe(this.RemoveShopFromShopsList);

            this.AddShopsToViewModel();
        }

        private void ShowCreateShopAction() => UIEvents.ShowCreateShopViewModel.Publish(null);
        private void ShowDeleteShopAction() => UIEvents.ShowDeleteShopViewModel.Publish(this.SelectedShop);

        private void AddItemsForShopAction(object parameter)
        {
            this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(
                 this.itemService.GetAllItemsForShop(this.SelectedShop));

            this.AreItemsShown = true;
            if (this.ItemsList.Any())
            {
                this.SelectedItem = this.ItemsList.First();
            }
        }

        private void AddShopToShopsList(object sender, OnlineShopModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.OnlineShops.Add(e);

                this.OnPropertyChanged(() => this.IsListOfShopsShown);
            });
        }

        private void RemoveShopFromShopsList(object sender, OnlineShopModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.OnlineShops.Remove(e);

                this.OnPropertyChanged(() => this.IsListOfShopsShown);
            });
        }

        private void AddShopsToViewModel()
        {
            this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(
                this.onlineShopService.GetAllShops());

            this.IsListOfShopsShown = this.OnlineShops.Any();
        }

        private bool ShowItemsPredicate()
        {
            return this.OnlineShops.Any();
        }
    }
}
