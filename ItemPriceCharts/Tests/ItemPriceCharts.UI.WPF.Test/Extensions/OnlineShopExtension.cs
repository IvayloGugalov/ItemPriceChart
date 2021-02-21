using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.Test.Extensions
{
    public static class OnlineShopExtension
    {
        public static OnlineShop ConstructOnlineShop(
            int id,
            string url,
            string title)
        {
            return OnlineShop.Construct(
                id,
                url,
                title);
        }
    }
}
