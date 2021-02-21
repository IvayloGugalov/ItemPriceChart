﻿using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShop : EntityModel
    {
        public string URL { get; }
        public string Title { get; }

        private List<Item> items = new List<Item>();
        public IReadOnlyCollection<Item> Items => this.items.AsReadOnly();

        private OnlineShop()
        {
        }

        public OnlineShop(string url, string title)
        {
            this.URL = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public void AddItem(Item item)
        {
            this.items.Add(item);
        }

        public void DeleteItem(Item item)
        {
            this.items.Remove(item);
        }

        public override string ToString()
        {
            return $"Id: {this.Id}, Url: {this.URL}, Title: {this.Title}";
        }


        private OnlineShop(
            int id,
            string url,
            string title)
        {
            this.Id = id;
            this.URL = url;
            this.Title = title;
        }

        /// <summary>
        /// Creating a mock shop for testing
        /// </summary>
        /// <returns>New shop</returns>
        public static OnlineShop Construct(
            int id,
            string url,
            string title)
        {
            return new OnlineShop(
                id,
                url,
                title);
        }
    }
}