using System.Linq;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemListingViewModel));

        public ItemListingViewModel(IItemService itemService)
            : base (itemService)
        {
            this.ShouldShowShopInformation = false;
        }

        /// <summary>
        /// Called from outside the class
        /// </summary>
        /// <returns></returns>
        public async Task SetItemsListAsync()
        {
            if (this.SelectedShop != null)
            {
                this.ItemsList = this.SelectedShop.Items.ToObservableCollection();
            }
            else
            {
                var retrievedItems = await this.ItemService.GetAllItems();

                this.ItemsList = retrievedItems.ToObservableCollection();
            }

            if (this.ItemsList is not null && this.ItemsList.Any())
            {
                this.AreItemsShown = true;
                this.SelectedItem = this.ItemsList.First();
            }
        }

    }
}
