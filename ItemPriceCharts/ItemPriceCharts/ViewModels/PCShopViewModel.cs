using System;
using ItemPriceCharts.ViewModels;
using Services.Models;
using Services.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UI.WPF.Helpers;
using System.Windows.Input;
using ItemPriceCharts.Helpers;

namespace UI.WPF.ViewModels
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
            this.AddShops();
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
                this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection<string>(onlineShopTitles);
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
