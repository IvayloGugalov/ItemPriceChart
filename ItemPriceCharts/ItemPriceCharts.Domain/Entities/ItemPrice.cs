using System;

namespace ItemPriceCharts.Domain.Entities
{
    public record ItemPrice
    {
        public DateTime PriceDateRetrieved { get; }
        public double Price { get; private set; }

        public ItemPrice(double price)
        {
            this.PriceDateRetrieved = DateTime.UtcNow;
            this.Price = price >= 0 ? price : throw new ArgumentNullException(nameof(price));
        }
    }
}
