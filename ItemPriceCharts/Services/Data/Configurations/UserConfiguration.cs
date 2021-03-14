using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();
            builder.Property(u => u.FirstName)
                .IsRequired();
            builder.Property(u => u.LastName)
                .IsRequired();
            builder.Property(u => u.Username)
                .IsRequired();
            builder.Property(u => u.Email)
                .IsRequired();
            builder.Property(u => u.Password)
                .IsRequired();
        }
    }
}
