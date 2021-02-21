using System;

namespace ItemPriceCharts.Services.Models
{
    public sealed partial class Item : EntityModel
    {
        public string Description { get; set; }
        public double CurrentPrice { get; set; }
        public string URL { get; }
        public string Title { get; }
        public ItemType Type { get; }

        public OnlineShop OnlineShop { get; }

        private Item()
        {
        }

        public Item(
            string url,
            string title,
            string description,
            double price,
            OnlineShop onlineShop,
            ItemType type)
        {
            this.URL = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
            this.Description = description;
            this.CurrentPrice = price >= 0 ? price : throw new ArgumentNullException(nameof(price));
            this.OnlineShop = onlineShop ?? throw new ArgumentNullException(nameof(onlineShop));
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Item other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Description != other.Description)
            {
                return false;
            }
            if (this.CurrentPrice != other.CurrentPrice)
            {
                return false;
            }
            if (this.URL != other.URL)
            {
                return false;
            }
            if (this.Title != other.Title)
            {
                return false;
            }
            if (this.Type != other.Type)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"Id: {this.Id}, Title:{this.Title}, Price: {this.CurrentPrice}, Type: {this.Type}, Shop id: {this.OnlineShop.Id}";
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(Item a, Item b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Item a, Item b)
        {
            return !(a == b);
        }

        private Item(
            int id,
            string url,
            string title,
            string description,
            double price,
            OnlineShop onlineShop,
            ItemType type)
        {
            this.Id = id;
            this.URL = url;
            this.Title = title;
            this.Description = description;
            this.CurrentPrice = price;
            this.OnlineShop = onlineShop;
            this.Type = type;
        }

        /// <summary>
        /// Creating a mock item for testing
        /// </summary>
        /// <returns>New item</returns>
        public static Item Construct(
            int id,
            string url,
            string title,
            string description,
            double price,
            OnlineShop onlineShop,
            ItemType type)
        {
            return new Item(
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
