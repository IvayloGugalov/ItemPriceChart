using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Events
{
    public static class EventsLocator
    {
        /// <summary>
        /// Event raised when a shop has been added
        /// </summary>
        public static IChannel<OnlineShop> ShopAdded { get; set; } = new Channel<OnlineShop>();
        /// <summary>
        /// Event raised when a shop has been deleted
        /// </summary>
        public static IChannel<OnlineShop> ShopDeleted { get; set; } = new Channel<OnlineShop>();

        /// <summary>
        /// Event raised when an item has been added
        /// </summary>
        public static IChannel<Item> ItemAdded { get; set; } = new Channel<Item>();

        /// <summary>
        /// Event raised when an item has been deleted
        /// </summary>
        public static IChannel<Item> ItemDeleted { get; set; } = new Channel<Item>();

    }
}
