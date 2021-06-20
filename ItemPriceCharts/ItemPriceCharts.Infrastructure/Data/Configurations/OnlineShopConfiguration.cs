using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data.Configurations
{
    public class OnlineShopConfiguration : IEntityTypeConfiguration<OnlineShop>
    {
        public void Configure(EntityTypeBuilder<OnlineShop> builder)
        {
            builder.HasKey(shop => shop.Id);

            builder.Property(shop => shop.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(shop => shop.Title)
                .IsRequired();

            builder.Property(shop => shop.Url)
                .IsRequired();

            builder.HasMany(shop => shop.Items)
                .WithOne(item => item.OnlineShop);

            builder.Metadata
                .FindNavigation(nameof(OnlineShop.Items))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
