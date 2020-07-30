using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShopModel
    {
        [Key]
        public int ShopId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public List<ItemModel> Items { get; } = new List<ItemModel>();

        public OnlineShopModel()
        {
        }

        public OnlineShopModel(int id, string url, string title)
            : this()
        {
            this.ShopId = id;
            this.Url = url;
            this.Title = title;
        }
    }
}
