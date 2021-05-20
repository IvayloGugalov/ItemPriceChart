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
        }
    }
}
