using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable(nameof(Item));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Title)
                .IsRequired();

            builder.Property(i => i.URL)
                .IsRequired();

            builder.Property(i => i.Description)
                .IsRequired();

            builder.Property(i => i.CurrentPrice)
                .IsRequired();

            builder.Property(i => i.Type)
                .HasConversion<int>();

            builder.HasOne(item => item.OnlineShop)
                .WithMany(shop => shop.Items)
                .IsRequired();
        }
    }
}
