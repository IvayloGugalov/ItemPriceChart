﻿using System;

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

        public Item(string url, string title, string description, double price, OnlineShop onlineShop, ItemType type)
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
            if (!(obj is Item other))
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
    }
}
