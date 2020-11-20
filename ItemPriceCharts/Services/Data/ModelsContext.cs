using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class ModelsContext : DbContext, IModelsContext
    {
        private readonly StreamWriter logStream = new StreamWriter(@"D:\Visual Studio Apps\ItemPriceChart\database.log", append: true);
        
        private bool disposed = false;

        public DbSet<OnlineShop> OnlineShops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }

        public IRepository<Item> ItemRepository => new Repository<Item>(this);
        public IRepository<OnlineShop> OnlineShopRepository => new Repository<OnlineShop>(this);
        public IRepository<ItemPrice> ItemPriceRepository => new Repository<ItemPrice>(this);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ItemPriceChartsDB.db")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(this.logStream.WriteLine,
                       Microsoft.Extensions.Logging.LogLevel.Debug,
                       DbContextLoggerOptions.DefaultWithUtcTime);
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

        public override void Dispose()
        {
            base.Dispose();
            this.logStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
