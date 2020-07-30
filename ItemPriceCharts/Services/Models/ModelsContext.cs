using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.Services.Models
{
    public class ModelsContext : DbContext
    {
        public DbSet<OnlineShopModel> OnlineShops { get; set; }
        public DbSet<ItemModel> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ItemPriceChartsDB.db");
        }
    }
}
