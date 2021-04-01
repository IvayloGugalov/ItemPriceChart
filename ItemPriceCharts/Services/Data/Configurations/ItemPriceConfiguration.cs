using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data.Configurations
{
    public class ItemPriceConfiguration : IEntityTypeConfiguration<ItemPrice>
    {
        public void Configure(EntityTypeBuilder<ItemPrice> builder)
        {
            builder.ToTable(nameof(ItemPrice));

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ItemId)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.PriceDate)
                .HasColumnType("DATE")
                .IsRequired();
        }
    }
}
