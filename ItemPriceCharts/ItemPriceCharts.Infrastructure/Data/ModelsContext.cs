using System;

using Microsoft.EntityFrameworkCore;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data
{
    public class ModelsContext : DbContext
    {
        public DbSet<OnlineShop> OnlineShops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserAccountOnlineShops> UserAccountOnlineShops { get; set; }
        public ModelsContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ModelsContext).Assembly);
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
