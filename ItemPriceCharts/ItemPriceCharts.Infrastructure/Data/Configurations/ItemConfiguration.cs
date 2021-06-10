using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();

            builder.Property(item => item.Title)
                .IsRequired();

            builder.Property(item => item.Url)
                .IsRequired();

            builder.Property(item => item.Description)
                .IsRequired();

            // Navigation property
            builder.OwnsOne(item => item.CurrentPrice);
            builder.Navigation(item => item.CurrentPrice)
                .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(item => item.OriginalPrice);
            builder.Navigation(item => item.OriginalPrice)
                .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(item => item.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(item => item.OnlineShop)
                .WithMany(shop => shop.Items)
                .IsRequired();

            builder.OwnsMany(item => item.PricesForItem);
            builder.Navigation(item => item.PricesForItem)
                .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
