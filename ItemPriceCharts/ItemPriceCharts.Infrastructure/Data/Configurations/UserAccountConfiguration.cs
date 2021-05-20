using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data.Configurations
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
                .ValueGeneratedNever();

            builder.HasKey(account => account.Id);

            builder.Property(account => account.Email)
                .HasColumnName("Email")
                .HasConversion(email => email.Value, value => new Email(value))
                .HasMaxLength(220)
                .IsRequired();
            //builder.OwnsOne(account => account.Email);
            //builder.Navigation(account => account.Email).IsRequired().AutoInclude();

            builder.Property(account => account.FirstName)
                .IsRequired();

            builder.Property(account => account.LastName)
                .IsRequired();

            builder.Property(account => account.Username)
                .IsRequired();

            builder.Property(account => account.Password)
                .IsRequired();

            //builder.HasMany(account => account.OnlineShopsForUser)
            //    .WithMany(onlineShop => onlineShop.UserAccounts)
            //    .UsingEntity(j => j.ToTable("UserAccountShops"));
        }
    }
}
