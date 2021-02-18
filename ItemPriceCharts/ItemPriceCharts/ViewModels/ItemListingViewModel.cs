using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        public ItemListingViewModel(ItemService itemService)
            : base (itemService)
        {
            this.ShouldShowShopInformation = false;
        }

        public async Task ShowItems()
        {
            this.ItemsList = await this.GetItems();

            this.AreItemsShown = true;
            if (this.ItemsList.Any())
            {
                this.SelectedItem = this.ItemsList.First();
            }
        }

        private ValueTask<ObservableCollection<Item>> GetItems()
        {
            Task.Delay(5000);
            if (this.SelectedShop is not null)
            {
                return new ValueTask<ObservableCollection<Item>>(ToObservableCollectionExtensions.ToObservableCollection(this.SelectedShop.Items));
            }
            else
            {
                return new ValueTask<ObservableCollection<Item>>(ToObservableCollectionExtensions.ToObservableCollection(this.ItemService.GetAllItems()));
            }
        }
    }
}
