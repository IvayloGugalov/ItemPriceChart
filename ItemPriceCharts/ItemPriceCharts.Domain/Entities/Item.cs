using System;
using System.Collections.Generic;

using ItemPriceCharts.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.Domain.Entities
{
    public sealed class Item : EntityModel
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ItemPrice CurrentPrice { get; private set; }
        public ItemPrice OriginalPrice { get; }
        public string Url { get; }
        public ItemType Type { get; }
        public OnlineShop OnlineShop { get; }

        public IReadOnlyCollection<ItemPrice> PricesForItem => this.pricesForItem.AsReadOnly();
        private readonly List<ItemPrice> pricesForItem;

        private Item() { }

        public Item(
            string url,
            string title,
            string description,
            ItemPrice price,
            OnlineShop onlineShop,
            ItemType type)
            : this()
        {
            this.Id = Guid.NewGuid();
            this.Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
            this.Description = description;
            this.CurrentPrice = price.Price >= 0 ? price : throw new ArgumentNullException(nameof(price));
            this.OriginalPrice = price.Price >= 0 ? price : throw new ArgumentNullException(nameof(price));
            this.OnlineShop = onlineShop ?? throw new ArgumentNullException(nameof(onlineShop));
            this.Type = type;

            this.pricesForItem = new List<ItemPrice>()
            {
                this.OriginalPrice,
                this.CurrentPrice
            };
        }

        public Item UpdateItem((string title, string description, ItemPrice price) updatedItem)
        {
            this.Title = updatedItem.title;
            this.Description = updatedItem.description;
            this.CurrentPrice = updatedItem.price;

            return this;
        }

        public Item UpdateItemPrice(ItemPrice updatedPrice, DbContext context = null)
        {
            if (this.pricesForItem != null)
            {
                this.pricesForItem.Add(updatedPrice);
                this.CurrentPrice = updatedPrice;
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "No context provided when pricesForItem collection is invalid.");
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(updatedPrice);
                this.CurrentPrice = updatedPrice;
            }

            return this;
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

            if (this.Url != other.Url)
            {
                return false;
            }

            if (this.Title != other.Title)
            {
                return false;
            }

            return this.Type == other.Type;
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
            Guid id,
            string url,
            string title,
            string description,
            ItemPrice price,
            OnlineShop onlineShop,
            ItemType type)
        {
            this.Id = id;
            this.Url = url;
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
            Guid id,
            string url,
            string title,
            string description,
            ItemPrice price,
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
