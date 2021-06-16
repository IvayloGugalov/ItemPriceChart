using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data.Configurations
{
    public class UserAccountOnlineShopsConfiguration : IEntityTypeConfiguration<UserAccountOnlineShops>
    {
        public void Configure(EntityTypeBuilder<UserAccountOnlineShops> builder)
        {
            builder.HasKey(x => new { x.UserAccountId, x.OnlineShopId});

            //If you name your foreign keys correctly, then you don't need this.
            builder.HasOne(pt => pt.UserAccount)
                .WithMany(p => p.OnlineShopsForUser)
                .HasForeignKey(pt => pt.UserAccountId);

            builder.HasOne(pt => pt.OnlineShop)
                .WithMany(t => t.UserAccounts)
                .HasForeignKey(pt => pt.OnlineShopId);
        }
    }
}
