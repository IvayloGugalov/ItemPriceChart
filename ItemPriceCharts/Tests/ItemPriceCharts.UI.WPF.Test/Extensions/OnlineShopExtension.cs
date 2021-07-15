using System;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.UI.WPF.Test.Extensions
{
    public static class OnlineShopExtension
    {
        public static OnlineShop ConstructOnlineShopWithParameters(
            Guid id,
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
                id: new Guid(),
                url: "https://www.someShop.com",
                title: "someShop");
        }
    }
}
