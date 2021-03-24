using System;
using System.Collections.Generic;

using NLog;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShop : EntityModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(OnlineShop));

        public string URL { get; }
        public string Title { get; }

        private readonly List<Item> items = new List<Item>();
        public IReadOnlyCollection<Item> Items => this.items.AsReadOnly();

        public ICollection<UserAccount> UserAccounts { get; set; }

        private OnlineShop() { }

        public OnlineShop(string url, string title) : this()
        {
            this.URL = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public void AddItem(Item item)
        {
            this.items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            this.items.Remove(item);
        }

        public void UpdateItem(Item updatedItem)
        {
            var item = this.items.Find(i => i.Id == updatedItem.Id);

            if (item != null)
            {
                this.items[this.items.FindIndex(i => i.Id == updatedItem.Id)] = updatedItem;

                logger.Info($"Updated item: {item.Title}:" +
                            $"\nFrom {item.Description} to {updatedItem.Description}" +
                            $"\nFrom {item.CurrentPrice} to {updatedItem.CurrentPrice}");
            }
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
