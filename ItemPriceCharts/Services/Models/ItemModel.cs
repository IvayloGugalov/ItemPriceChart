using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public sealed class ItemModel
    {
        [Key]
        public int Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public OnlineShopModel OnlineShop { get; set; }

        public ItemModel()
        {
        }

        public ItemModel(int id, string url, string title, string description, double price, OnlineShopModel onlineShop)
            : this()
        {
            this.Id = id;
            this.URL = url;
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.OnlineShop = onlineShop;
        }
    }
}
