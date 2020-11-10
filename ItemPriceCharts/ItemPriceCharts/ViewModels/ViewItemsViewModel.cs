using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ViewItemsViewModel : ShopViewModel
    {
        private readonly IItemService itemService;

        public ViewItemsViewModel(ItemService itemService, OnlineShopService onlineShopService)
            : base (itemService)
        {
            this.itemService = itemService;

            this.ShouldShowShopInformation = false;

            this.ShowAllItems();
        }

        private void ShowAllItems()
        {
            this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(this.itemService.GetAllItems());

            this.AreItemsShown = true;
            if (this.ItemsList.Any())
            {
                this.SelectedItem = this.ItemsList.First();
            }
        }
    }
}
