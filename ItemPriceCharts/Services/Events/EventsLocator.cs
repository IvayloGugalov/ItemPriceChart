using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Events
{
    public static class EventsLocator
    {
        /// <summary>
        /// Event raised when a shop has been added
        /// </summary>
        public static IChannel<OnlineShopModel> ShopAdded { get; set; } = new Channel<OnlineShopModel>();
        /// <summary>
        /// Event raised when a shop has been deleted
        /// </summary>
        public static IChannel<OnlineShopModel> ShopDeleted { get; set; } = new Channel<OnlineShopModel>();

        /// <summary>
        /// Event raised when an item has been added
        /// </summary>
        public static IChannel<ItemModel> ItemAdded { get; set; } = new Channel<ItemModel>();

        /// <summary>
        /// Event raised when an item has been deleted
        /// </summary>
        public static IChannel<ItemModel> ItemDeleted { get; set; } = new Channel<ItemModel>();

    }
}
