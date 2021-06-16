using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Data.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.HasKey(account => account.Id);

            builder.ToTable("UserAccounts")
                .HasIndex(account => new
                {
                    account.Username,
                    account.Email
                })
                .IsUnique();

            builder.HasAlternateKey(account => new
            {
                account.Username, account.Email
            });

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

            //builder.Metadata
            //    .FindNavigation(nameof(UserAccount.OnlineShopsForUser))
            //    .SetPropertyAccessMode(PropertyAccessMode.Field);

            //builder.HasMany(account => account.OnlineShopsForUser)
            //    .WithMany(shop => shop.UserAccounts)
            //    .UsingEntity(j => j.ToTable("UserAccountOnlineShops"));


            //builder
            //    .HasMany(userAccount => userAccount.OnlineShopsForUser)
            //    .WithMany(shop => shop.UserAccounts)
            //    .UsingEntity<OnlineShopsForUser>(
            //        typeBuilder => typeBuilder.HasOne(prop => prop.OnlineShop)
            //            .WithMany()
            //            .HasForeignKey(prop => prop.OnlineShopId),
            //        typeBuilder => typeBuilder.HasOne(prop => prop.UserAccount)
            //            .WithMany()
            //            .HasForeignKey(prop => prop.UserAccountId),
            //        typeBuilder => typeBuilder.HasKey(prop =>
            //            new { prop.UserAccountId, prop.OnlineShopId })
            //    );
        }
    }
}
