using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.Entities
{
    public class OnlineShop
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public List<Item> Items { get; } = new List<Item>();

        public OnlineShop(string title, string url)
        {
            this.Title = title;
            this.Url = url;
        }
    }
}
