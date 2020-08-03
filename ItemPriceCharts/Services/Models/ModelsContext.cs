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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ItemModel>()
                .HasOne(item => item.OnlineShop)
                .WithMany(shop => shop.Items)
                .IsRequired();
        }
    }
}
