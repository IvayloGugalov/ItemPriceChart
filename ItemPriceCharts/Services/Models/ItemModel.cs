namespace ItemPriceCharts.Services.Models
{
    public sealed partial class ItemModel: EntityModel
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public ItemType Type { get; set; }

        public OnlineShopModel OnlineShop { get; set; }

        public ItemModel()
        {
        }

        public ItemModel(int id, string url, string title, string description, double price, OnlineShopModel onlineShop, ItemType type)
            : this()
        {
            this.Id = id;
            this.URL = url;
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.OnlineShop = onlineShop;
            this.Type = type;
        }
    }
}
