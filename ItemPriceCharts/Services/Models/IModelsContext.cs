using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.Services.Models
{
    public interface IModelsContext
    {
        DbSet<OnlineShopModel> OnlineShops { get; set; }
        DbSet<ItemModel> Items { get; set; }
        DbSet<ItemPrice> ItemPrices { get; set; }
        DbSet<EntityModel> Entities { get; set; }
    }
}
