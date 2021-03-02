using System;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.Test.Extensions
{
    public static class ItemExtension
    {
        public static Item ConstructItem(
            int id,
            string url,
            string title,
            string description,
            double price,
            OnlineShop onlineShop,
            ItemType type)
        {
            return Item.Construct(
                id,
                url,
                title,
                description,
                price,
                onlineShop,
                type);
        }

        /// <summary>
        /// Constructs a default item with specified parameters
        /// </summary>
        /// <returns></returns>
        public static Item ConstructDefaultItem(OnlineShop onlineShop)
        {
            return Item.Construct(
                id: new Random(10000).Next(),
                url: string.Concat(onlineShop.URL, "//", new Random(10000).Next().ToString()),
                title: "firstItem",
                description: "item description",
                price: new Random(10000).NextDouble(),
                onlineShop: onlineShop,
                type: ItemType.ComputerItem);
        }
    }
}
