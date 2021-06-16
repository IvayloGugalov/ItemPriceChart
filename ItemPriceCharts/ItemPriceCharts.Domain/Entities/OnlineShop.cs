using System;
using System.Collections.Generic;

using ItemPriceCharts.Domain.Events;

namespace ItemPriceCharts.Domain.Entities
{
    public sealed class OnlineShop : EntityModel
    {
        public string Url { get; }
        public string Title { get; }

        public IReadOnlyCollection<Item> Items => this.items?.AsReadOnly();
        private readonly List<Item> items = new();

        public IReadOnlyCollection<UserAccountOnlineShops> UserAccounts => this.userAccounts?.AsReadOnly();
        private readonly List<UserAccountOnlineShops> userAccounts = new();

        private OnlineShop() { }

        public OnlineShop(string url, string title)
            : this()
        {
            this.Id = Guid.NewGuid();
            this.Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public void AddItem(Item item)
        {
            if (this.items == null)
            {
                throw new InvalidOperationException("Could not add an item.");
            }

            this.items.Add(item);

            DomainEvents.ItemAdded.Raise(item);
        }

        public void RemoveItem(Item item)
        {
            if (this.items == null)
            {
                throw new NullReferenceException("You must use .Include(p => p.Items) before calling this method.");
            }

            this.items.Remove(item);
            DomainEvents.ItemDeleted.Raise(item);
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
