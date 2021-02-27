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

        /// <summary>
        /// Constructs a default online shop with specified parameters
        /// </summary>
        /// <returns></returns>
        public static OnlineShop ConstructDefaultOnlineShop()
        {
            return OnlineShop.Construct(
                id: 1,
                url: "https://www.someShop.com",
                title: "someShop");
        }
    }
}
