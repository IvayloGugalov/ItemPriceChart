using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.InfraStructure.Constants;

namespace ItemPriceCharts.Infrastructure.Data
{
    public class ModelsContext : DbContext
    {
        private readonly StreamWriter logStream = new(
            string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseKeyWordConstants.DATABASE_LOG_PATH),
            append: true);

        private IDbContextTransaction transaction;

        public DbSet<OnlineShop> OnlineShops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserAccountOnlineShops> UserAccountOnlineShops { get; set; }

        public void BeginTransaction()
        {
            this.transaction = Database.BeginTransaction();
        }

        public void CommitToDatabase()
        {
            try
            {
                this.SaveChanges();
                this.transaction.Commit();
            }
            finally
            {
                this.transaction.Dispose();
            }
        }

        public void Rollback()
        {
            this.transaction.Rollback();
            this.transaction.Dispose();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseKeyWordConstants.CONNECTION_STRING)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .LogTo(this.logStream.WriteLine,
                       Microsoft.Extensions.Logging.LogLevel.Information,
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
