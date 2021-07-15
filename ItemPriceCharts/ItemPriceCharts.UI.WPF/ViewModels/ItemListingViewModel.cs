using System.Linq;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Extensions;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ItemListingViewModel));

        public ItemListingViewModel(UserAccount userAccount, UiEvents uiEvents)
            : base (userAccount, uiEvents)
        {
            this.ShouldShowShopInformation = false;
        }

        //Called from outside the class
        public void SetItemsList()
        {
            this.ItemsList = this.SelectedShop != null
                ? this.SelectedShop.Items
                    .ToObservableCollection()
                : this.UserAccount.OnlineShopsForUser
                    .Select(x => x.OnlineShop)
                    .SelectMany(shop => shop.Items)
                    .ToObservableCollection();

            if (this.ItemsList is not null && this.ItemsList.Any())
            {
                this.AreItemsShown = true;
                this.SelectedItem = this.ItemsList.First();
            }
        }

    }
}
