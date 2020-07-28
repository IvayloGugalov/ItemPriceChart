using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.Entities
{
    public class ItemResult
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

        public ItemResult(string title, string price, string description)
        {
            this.Title = title;
            this.Price = price;
            this.Description = description;
        }
    }
}
