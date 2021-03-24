using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data.Configurations
{
    public class OnlineShopConfiguration : IEntityTypeConfiguration<OnlineShop>
    {
        public void Configure(EntityTypeBuilder<OnlineShop> builder)
        {
            builder.ToTable(nameof(OnlineShop));

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Title)
                .IsRequired();

            builder.Property(s => s.URL)
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne(i => i.OnlineShop);

            builder.HasMany(s => s.UserAccounts);
        }
    }
}
