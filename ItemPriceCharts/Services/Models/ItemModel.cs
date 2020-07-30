using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public sealed class ItemModel
    {
        [Key]
        public int ItemtId { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public int OnlineShopModelId { get; set; }
        public OnlineShopModel OnlineShop { get; set; }

        public ItemModel()
        {
        }

        public ItemModel(int id, string url, string title, string description, double price, int onlineShopId)
            : this()
        {
            this.ItemtId = id;
            this.URL = url;
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.OnlineShopModelId = onlineShopId;
        }
    }
}
