using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class PCShopViewModel : ShopViewModel
    {
        private readonly WebPageService webPageService;

        public ObservableCollection<ItemViewModel> ListOfItems { get; set; } = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<string> OnlineShops { get; set; }

        public ICommand ShowItemsCommand { get; }

        public PCShopViewModel(WebPageService webPageService)
            : base(webPageService)
        {
            this.webPageService = webPageService;

            this.ShowItemsCommand = new RelayCommand(_ => this.ShowItemsAction());

            //PublishSubscribe<object>.RegisterEvent("NewShopAdded", this.UpdateListViewHandler);
            NewEvents.NewShopAddedSub.Publisher.OnDataPublished += this.UpdateListViewHandler;

            this.AddShops();
        }

        //private void UpdateListViewHandler(object sender, PublishAndSubscribeEventArgs<object> args)
        //{
        //    this.OnlineShops.Add(args.Item.ToString());
        //}

        private void UpdateListViewHandler(object sender, MessageArgument<string> e)
        {
            this.OnlineShops.Add(e.Message);
        }

        private void ShowItemsAction()
        {
            //show list of items
        }

        private void AddShops()
        {
            var onlineShopTitles = new List<string>();
            foreach (var onlineShop in this.webPageService.RetrieveOnlineShops())
            {
                onlineShopTitles.Add(onlineShop.Title);
            }

            if (onlineShopTitles.Any())
            {
                this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(onlineShopTitles);
            }
        }

        private void AddItems()
        {
            var results = this.webPageService.FindRequiredTextForPC();

            foreach (var result in results)
            {
                this.ListOfItems.Add(new ItemViewModel(result));
            }

            if (results.Any())
            {
                this.IsChartShown = true;
            }
        }
    }
}
