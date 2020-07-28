using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public sealed class ItemModel
    {
        [Key]
        public int ItemtId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string URL { get; set; }

        public int OnlineShopModelId { get; set; }
        public OnlineShopModel OnlineShop { get; set; }
    }
}
