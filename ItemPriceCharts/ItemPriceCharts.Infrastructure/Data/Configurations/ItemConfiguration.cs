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

            builder.Property(item => item.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(item => item.OnlineShop)
                .WithMany(shop => shop.Items)
                .IsRequired();

            // Navigation properties
            builder.OwnsOne(item => item.CurrentPrice)
                .Property(itemPrice => itemPrice.Price)
                .HasColumnType("double")
                .IsRequired();

            builder.OwnsOne(item => item.OriginalPrice)
                .Property(itemPrice => itemPrice.Price)
                .HasColumnType("double")
                .IsRequired();

            builder.OwnsMany(item => item.PricesForItem,
                x =>
                {
                    x.WithOwner()
                        .HasForeignKey(nameof(ItemPrice.ItemId));
                    x.Property<int>("Id");
                    x.HasKey("Id");
                    
                    x.Property(a => a.Price)
                        .HasColumnType("double")
                        .IsRequired();
                    x.Property(a => a.PriceDateRetrieved)
                        .HasColumnType("date")
                        .ValueGeneratedNever()
                        .IsRequired();
                });

            builder.Navigation(item => item.PricesForItem)
                .Metadata.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
