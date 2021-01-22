using System.Linq;

using NLog;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemListingViewModel));

        public OnlineShop OnlineShop { get; set; }

        public ItemListingViewModel(ItemService itemService)
            : base (itemService)
        {
            this.ShouldShowShopInformation = false;
        }

        public void ShowItems()
        {
            try
            {
                if (this.OnlineShop is not null)
                {
                    this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(this.OnlineShop.Items);
                }
                else
                {
                    this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(this.ItemService.GetAllItems());
                }

                this.AreItemsShown = true;
                if (this.ItemsList.Any())
                {
                    this.SelectedItem = this.ItemsList.First();
                }
            }
            catch (System.Exception e)
            {
                logger.Info($"Failed to get items.\t{e}");
                UIEvents.ShowMessageDialog(
                        new MessageDialogViewModel(
                            title: "Error",
                            description: e.Message,
                            buttonType: ButtonType.Close));
            }
        }
    }
}
