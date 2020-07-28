using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.Entities
{
    public class Item
    {
        public int ItemtId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string URL { get; set; }

        public int OnlineShopId { get; set; }

        public Item(string title, string description, double price, string url, int shopId)
        {
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.URL = url;
            this.OnlineShopId = shopId;
        }
    }
}
