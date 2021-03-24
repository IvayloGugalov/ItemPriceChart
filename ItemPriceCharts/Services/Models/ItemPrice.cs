using System;

namespace ItemPriceCharts.Services.Models
{
    public sealed class ItemPrice : EntityModel
    {
        public DateTime PriceDate { get; }
        public double Price { get; }
        public int ItemId { get; }

        private ItemPrice() { }

        public ItemPrice(double currentPrice, int itemId) : this()
        {
            this.PriceDate = DateTime.Now;
            this.Price = currentPrice >= 0 ? currentPrice : throw new ArgumentNullException(nameof(currentPrice));
            this.ItemId = itemId;
        }
    }
}
