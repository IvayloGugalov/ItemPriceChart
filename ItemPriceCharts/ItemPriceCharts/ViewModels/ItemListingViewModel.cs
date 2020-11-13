using System.Linq;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private readonly IItemService itemService;

        public ItemListingViewModel(ItemService itemService, OnlineShopService onlineShopService)
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
