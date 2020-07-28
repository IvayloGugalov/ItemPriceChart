using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public sealed class OnlineShopModel
    {
        [Key]
        public int ShopId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public List<ItemModel> Items { get; } = new List<ItemModel>();
    }
}
