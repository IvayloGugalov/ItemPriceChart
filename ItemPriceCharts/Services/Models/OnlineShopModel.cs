using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShopModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public ICollection<ItemModel> Items { get; set; }

        public OnlineShopModel()
        {
        }

        public OnlineShopModel(int id, string url, string title)
            : this()
        {
            this.Id = id;
            this.Url = url;
            this.Title = title;
        }
    }
}
