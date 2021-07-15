using System;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;

namespace ItemPriceCharts.UI.WPF.Test.Extensions
{
    public static class ItemExtension
    {
        public static Item ConstructItemWithParameters
        (
            Guid id,
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
        public static Item ConstructItem(OnlineShop onlineShop)
        {
            return Item.Construct(
                id: new Guid(),
                url: string.Concat(onlineShop.Url, "//", new Random(10000).Next().ToString()),
                title: "firstItem",
                description: "item description",
                price: new Random(10000).NextDouble(),
                onlineShop: onlineShop,
                type: ItemType.ComputerItem);
        }
    }
}
