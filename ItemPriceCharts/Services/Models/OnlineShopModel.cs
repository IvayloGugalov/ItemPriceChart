using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public sealed class OnlineShopModel: EntityModel
    {
        public ICollection<ItemModel> Items { get; set; }

        public OnlineShopModel()
        {
        }

        public OnlineShopModel(int id, string url, string title)
            : this()
        {
            this.Id = id;
            this.URL = url;
            this.Title = title;
        }
    }
}
