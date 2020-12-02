using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class ModelsContext : DbContext
    {
        private readonly StreamWriter logStream = new StreamWriter(
            string.Concat(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), @"\ItemPriceCharts\database.log"), append: true);

        public DbSet<OnlineShop> OnlineShops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ItemPriceChartsDB.db")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(this.logStream.WriteLine,
                       Microsoft.Extensions.Logging.LogLevel.Error,
                       DbContextLoggerOptions.DefaultWithUtcTime);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ModelsContext).Assembly);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.logStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
