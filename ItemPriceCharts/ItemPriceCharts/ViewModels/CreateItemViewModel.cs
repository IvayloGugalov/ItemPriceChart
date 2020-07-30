using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BindableViewModel
    {
        private readonly CreateItemView view;
        private readonly WebPageService webPageService;
        private string newItemURL;
        private string selectedShop;

        private readonly Dictionary<string, int> onlineShops = new Dictionary<string, int>();

        public string NewItemURL
        {
            get => this.newItemURL;
            set => this.SetValue(ref this.newItemURL, value);
        }

        public string SelectedShop
        {
            get => this.selectedShop;
            set => this.SetValue(ref this.selectedShop, value);
        }

        public ObservableCollection<string> OnlineShopsList { get; set; }

        public ICommand AddItemCommand { get; }

        public CreateItemViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.AddItemCommand = new RelayCommand<object>(this.AddItemAction, this.AddItemPredicate);
            this.GetShops();

            this.view = new CreateItemView(this);
            view.ShowDialog();

        }

        private void GetShops()
        {
            var onlineShopTitles = new List<string>();
            foreach (var onlineShop in this.webPageService.RetrieveOnlineShops())
            {
                onlineShopTitles.Add(onlineShop.Title);
                this.onlineShops.Add(onlineShop.Title, onlineShop.ShopId);
            }

            if (onlineShopTitles.Any())
            {
                this.OnlineShopsList = ToObservableCollectionExtensions.ToObservableCollection(onlineShopTitles);
            }
        }

        private void AddItemAction(object obj)
        {
            try
            {
                this.onlineShops.TryGetValue(this.SelectedShop, out var shopId);
                this.webPageService.CreateItem(this.NewItemURL, shopId);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.view.Close();
            }
        }


        private bool AddItemPredicate()
        {
            if (this.NewItemURL != null && this.SelectedShop != null)
            {
                return this.NewItemURL.Contains(this.SelectedShop.ToLower());
            }

            return false;
        }


        private bool IsCorrctURL()
        {
            return this.NewItemURL.EndsWith(".com") || this.NewItemURL.EndsWith(@".bg/");
        }
    }
}
