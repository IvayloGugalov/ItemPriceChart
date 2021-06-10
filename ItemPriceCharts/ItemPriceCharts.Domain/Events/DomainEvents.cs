
using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Domain.Events
{
    public static class DomainEvents
    {
        public static IDomainEvent<Item> ItemAdded { get; set; } = new DomainEvent<Item>();
        public static IDomainEvent<Item> ItemDeleted { get; set; } = new DomainEvent<Item>();
        public static IDomainEvent<OnlineShop> ShopAdded { get; set; } = new DomainEvent<OnlineShop>();
        public static IDomainEvent<OnlineShop> ShopDeleted { get; set; } = new DomainEvent<OnlineShop>();
    }
}
