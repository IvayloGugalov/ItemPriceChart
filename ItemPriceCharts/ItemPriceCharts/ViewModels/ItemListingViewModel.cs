using System.Linq;

using NLog;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            try
            {
                this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(this.itemService.GetAllItems());

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
