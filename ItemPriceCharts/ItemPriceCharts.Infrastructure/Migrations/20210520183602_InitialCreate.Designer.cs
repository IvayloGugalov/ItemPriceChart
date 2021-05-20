﻿// <auto-generated />
using System;
using ItemPriceCharts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItemPriceCharts.Infrastructure.Migrations
{
    [DbContext(typeof(ModelsContext))]
    [Migration("20210520183602_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OnlineShopId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OnlineShopId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.OnlineShop", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OnlineShops");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("Id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(220)
                        .HasColumnType("TEXT")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("UserAccount");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.UserAccountOnlineShops", b =>
                {
                    b.Property<Guid>("UserAccountId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OnlineShopId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserAccountId", "OnlineShopId");

                    b.HasIndex("OnlineShopId");

                    b.ToTable("UserAccountOnlineShops");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.Item", b =>
                {
                    b.HasOne("ItemPriceCharts.Domain.Entities.OnlineShop", "OnlineShop")
                        .WithMany("Items")
                        .HasForeignKey("OnlineShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ItemPriceCharts.Domain.Entities.ItemPrice", "CurrentPrice", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("Price")
                                .HasColumnType("REAL");

                            b1.HasKey("ItemId");

                            b1.ToTable("Items");

                            b1.WithOwner()
                                .HasForeignKey("ItemId");
                        });

                    b.OwnsOne("ItemPriceCharts.Domain.Entities.ItemPrice", "OriginalPrice", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("TEXT");

                            b1.Property<double>("Price")
                                .HasColumnType("REAL");

                            b1.HasKey("ItemId");

                            b1.ToTable("Items");

                            b1.WithOwner()
                                .HasForeignKey("ItemId");
                        });

                    b.OwnsMany("ItemPriceCharts.Domain.Entities.ItemPrice", "PricesForItem", b1 =>
                        {
                            b1.Property<Guid>("ItemId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<double>("Price")
                                .HasColumnType("REAL");

                            b1.HasKey("ItemId", "Id");

                            b1.ToTable("Items_PricesForItem");

                            b1.WithOwner()
                                .HasForeignKey("ItemId");
                        });

                    b.Navigation("CurrentPrice");

                    b.Navigation("OnlineShop");

                    b.Navigation("OriginalPrice");

                    b.Navigation("PricesForItem");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.UserAccountOnlineShops", b =>
                {
                    b.HasOne("ItemPriceCharts.Domain.Entities.OnlineShop", "OnlineShop")
                        .WithMany("UserAccounts")
                        .HasForeignKey("OnlineShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItemPriceCharts.Domain.Entities.UserAccount", "UserAccount")
                        .WithMany("OnlineShopsForUser")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OnlineShop");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.OnlineShop", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("UserAccounts");
                });

            modelBuilder.Entity("ItemPriceCharts.Domain.Entities.UserAccount", b =>
                {
                    b.Navigation("OnlineShopsForUser");
                });
#pragma warning restore 612, 618
        }
    }
}
