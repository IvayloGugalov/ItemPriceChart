using System;

using Microsoft.EntityFrameworkCore;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class ModelsContext : DbContext, IModelsContext
    {
        private bool disposed = false;

        public DbSet<OnlineShop> OnlineShops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }

        public IRepository<Item> ItemRepository => new Repository<Item>(this);
        public IRepository<OnlineShop> OnlineShopRepository => new Repository<OnlineShop>(this);
        public IRepository<ItemPrice> ItemPriceRepository => new Repository<ItemPrice>(this);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ItemPriceChartsDB.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ModelsContext).Assembly);
        }

        public void CommitChangesAsync()
        {
           this.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Dispose();
                }
            }
            this.disposed = true;
        }

        new public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
