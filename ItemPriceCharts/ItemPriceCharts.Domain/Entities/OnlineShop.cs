using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.Domain.Entities
{
    public sealed class OnlineShop : EntityModel
    {
        public string Url { get; }
        public string Title { get; }

        public IReadOnlyCollection<Item> Items => this.items?.AsReadOnly();
        private readonly List<Item> items = new();

        public ICollection<UserAccountOnlineShops> UserAccounts { get; set; }

        private OnlineShop() { }

        public OnlineShop(string url, string title)
            : this()
        {
            this.Id = Guid.NewGuid();
            this.Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public void AddItem(Item item, DbContext context = null)
        {
            if (this.items != null)
            {
                this.items.Add(item);
            }
            else if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "No context provided when Item collection is invalid.");
            }
            else if (context.Entry(this).IsKeySet)
            {
                context.Add(item);
            }
            else
            {
                throw new InvalidOperationException("Could not add an item.");
            }
        }

        public void RemoveItem(Item item)
        {
            if (this.items == null)
            {
                throw new NullReferenceException("You must use .Include(p => p.Items) before calling this method.");
            }

            this.items.Remove(item);
        }

        public override string ToString()
        {
            return $"Id: {this.Id}, Url: {this.Url}, Title: {this.Title}";
        }

        private OnlineShop(
            Guid id,
            string url,
            string title)
        {
            this.Id = id;
            this.Url = url;
            this.Title = title;
        }

        /// <summary>
        /// Creating a mock shop for testing
        /// </summary>
        /// <returns>New shop</returns>
        public static OnlineShop Construct(
            Guid id,
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
