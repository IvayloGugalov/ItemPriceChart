using System;

using System.Linq;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class PCShopViewModel : ShopViewModel
    {
        private readonly WebPageService webPageService;
       
        public ICommand ShowItemsCommand { get; }
        public ICommand ShowItemInformationDialogCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public PCShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.ShowItemsCommand = new RelayCommand(_ => this.AddItemsForShopAction());
            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            Events.ShopAdded.Subscribe(this.UpdateShopsListHandler);
            Events.ShopDeleted.Subscribe(this.UpdateShopsListHandler);
            Events.ItemAdded.Subscribe(this.UpdateItemsListHandler);
            Events.ItemDeleted.Subscribe(this.UpdateItemsListHandler);

            this.AddShopsToViewModel();
        }

        private void UpdateItemsListHandler(object sender, MessageArgument<ItemModel> e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (this.webPageService.TryGetItem(e.Message))
                {
                    this.ItemsList.Add(e.Message);
                }
                else
                {
                    this.ItemsList.Remove(e.Message);
                }
            });
        }

        private void UpdateShopsListHandler(object sender, MessageArgument<OnlineShopModel> e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                if (this.webPageService.TryGetShop(e.Message))
                {
                    this.OnlineShops.Add(e.Message);
                }
                else
                {
                    this.OnlineShops.Remove(e.Message);
                }
            });
        }

        private void AddShopsToViewModel()
        {
            this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(
                this.webPageService.RetrieveOnlineShops());
        }

        private void AddItemsForShopAction()
        {
            this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(
                 this.webPageService.RetrieveItemsForShop(this.SelectedShop));

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
            
        }
    }
}
