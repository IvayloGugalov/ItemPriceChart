using System;

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
       
        public ICommand ShowItemsCommand { get; }

        public PCShopViewModel(WebPageService webPageService)
            : base(webPageService)
        {
            this.webPageService = webPageService;

            this.ShowItemsCommand = new RelayCommand(_ => this.AddItemsForShop());

            Events.ShopAdded.Subscribe(this.UpdateListViewHandler);
            Events.ShopDeleted.Subscribe(this.UpdateListViewHandler);

            this.AddShopsToViewModel();
        }

        private void UpdateListViewHandler(object sender, MessageArgument<OnlineShopModel> e)
        {
            if (this.webPageService.TryGetShop(e.Message))
            {
                this.OnlineShops.Add(e.Message);
            }
            else
            {
                this.OnlineShops.Remove(e.Message);
            }
        }

        private void AddItemsForShop()
        {
            this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(
                this.webPageService.RetrieveItemsForShop().Select(item => new ItemViewModel(item)));
            this.AreItemsShown = true;
        }

        private void AddShopsToViewModel()
        {
             this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(this.webPageService.RetrieveOnlineShops());
        }
    }
}
