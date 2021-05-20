using System.Linq;

using NLog;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemListingViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemListingViewModel));

        public ItemListingViewModel(UserAccount userAccount)
            : base (userAccount)
        {
            this.ShouldShowShopInformation = false;
        }

        //Called from outside the class
        public void SetItemsList()
        {
            if (this.SelectedShop != null)
            {
                this.ItemsList = this.SelectedShop.Items.ToObservableCollection();
            }
            else
            {
                this.ItemsList = this.UserAccount.OnlineShopsForUser.Select(x => x.OnlineShop).SelectMany(shop => shop.Items).ToObservableCollection();
            }

            if (this.ItemsList is not null && this.ItemsList.Any())
            {
                this.AreItemsShown = true;
                this.SelectedItem = this.ItemsList.First();
            }
        }

    }
}
