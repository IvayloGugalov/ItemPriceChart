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
    public class PCShopViewModel : ShopViewModel
    {
        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;
       
        public ICommand ShowItemsCommand { get; }
        public ICommand ShowItemInformationDialogCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public PCShopViewModel(ItemService itemService, OnlineShopService onlineShopService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;

            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);
            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            EventsLocator.ShopAdded.Subscribe(this.UpdateShopsListHandler);
            EventsLocator.ShopDeleted.Subscribe(this.UpdateShopsListHandler);
            EventsLocator.ItemAdded.Subscribe(this.UpdateItemsListHandler);
            EventsLocator.ItemDeleted.Subscribe(this.UpdateItemsListHandler);

            this.AddShopsToViewModel();
        }

        private void UpdateItemsListHandler(object sender, ItemModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                if (this.itemService.IsItemExisting(e.Id))
                {
                    this.ItemsList.Add(e);
                }
                else
                {
                    this.ItemsList.Remove(e);
                }
            });
        }

        private void UpdateShopsListHandler(object sender, OnlineShopModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                if (this.onlineShopService.IsShopExisting(e.Id))
                {
                    this.OnlineShops.Add(e);
                }
                else
                {
                    this.OnlineShops.Remove(e);
                }

                this.OnPropertyChanged(() => this.IsListOfShopsShown);
                this.OnPropertyChanged(() => this.OnlineShops);
            });
        }

        private void AddShopsToViewModel()
        {
            this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(
                this.onlineShopService.GetAllShops());

            this.IsListOfShopsShown = this.OnlineShops.Any();
        }

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

        private void ShowItemInformationDialogAction()
        {
            UIEvents.ShowItemInformatioViewModel.Publish(this.SelectedItem);
        }

        private void DeleteItemAction()
        {
            this.itemService.DeleteItem(this.SelectedItem);
        }

        private bool ShowItemsPredicate()
        {
            return this.OnlineShops.Any();
        }
    }
}
