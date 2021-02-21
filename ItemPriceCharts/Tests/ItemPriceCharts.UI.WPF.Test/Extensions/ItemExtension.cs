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
    }
}
