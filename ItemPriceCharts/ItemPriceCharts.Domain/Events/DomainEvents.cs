
using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Domain.Events
{
    public static class DomainEvents
    {
        public static IEvent<Item> ItemAdded { get; set; } = new Event<Item>();
        public static IEvent<Item> ItemDeleted { get; set; } = new Event<Item>();
        public static IEvent<OnlineShop> ShopAdded { get; set; } = new Event<OnlineShop>();
        public static IEvent<OnlineShop> ShopDeleted { get; set; } = new Event<OnlineShop>();
    }
}
