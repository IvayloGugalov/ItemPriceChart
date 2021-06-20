using System;

namespace ItemPriceCharts.Domain.Entities
{
    public record ItemPrice
    {
        public DateTime PriceDateRetrieved { get; }
        public double Price { get; }

        public Guid ItemId { get; protected set; }

        private ItemPrice() { }

        public ItemPrice(Guid itemId, double price)
            : this()
        {
            this.PriceDateRetrieved = DateTime.UtcNow;
            this.Price = price >= 0 ? price : throw new ArgumentNullException(nameof(price));

            this.ItemId = itemId;
        }
    }
}
