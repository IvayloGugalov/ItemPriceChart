using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable(nameof(UserAccount))
                .HasIndex(account => new
                {
                    account.Username,
                    account.Email
                })
                .IsUnique();

            builder.HasKey(account => account.Id);

            builder.Property(account => account.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.HasKey(account => account.Id);

            builder.Property(account => account.Email)
                .HasColumnName("Email")
                .HasConversion(email => email.Value, value => new Email(value))
                .HasMaxLength(220)
                .IsRequired();

            builder.Property(account => account.FirstName)
                .IsRequired();

            builder.Property(account => account.LastName)
                .IsRequired();

            builder.Property(account => account.Username)
                .IsRequired();

            builder.Property(account => account.Password)
                .IsRequired();

            builder.HasMany(account => account.OnlineShopsForAccount);
        }
    }
}
