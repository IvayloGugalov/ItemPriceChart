using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShop: EntityModel
    {
        public string URL { get; set; }
        public string Title { get; set; }

        public ICollection<Item> Items { get; set; }

        private OnlineShop()
        {
        }

        public OnlineShop(string url, string title)
        {
            this.URL = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }
    }
}
