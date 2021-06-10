﻿// <auto-generated />
using System;
using ItemPriceCharts.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItemPriceCharts.Services.Migrations
{
    [DbContext(typeof(ModelsContext))]
    [Migration("20210324140942_CreateUserAccountEntity")]
    partial class CreateUserAccountEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ItemPriceCharts.Services.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OnlineShopId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("OnlineShopId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("ItemPriceCharts.Services.Models.ItemPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("PriceDate")
                        .HasColumnType("DATE");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("ItemPrice");
                });

            modelBuilder.Entity("ItemPriceCharts.Services.Models.OnlineShop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("OnlineShop");
                });

            modelBuilder.Entity("ItemPriceCharts.Services.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
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

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("UserAccount");
                });

            modelBuilder.Entity("OnlineShopUserAccount", b =>
                {
                    b.Property<int>("OnlineShopsForAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserAccountsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("OnlineShopsForAccountId", "UserAccountsId");

                    b.HasIndex("UserAccountsId");

                    b.ToTable("OnlineShopUserAccount");
                });

            modelBuilder.Entity("ItemPriceCharts.Services.Models.Item", b =>
                {
                    b.HasOne("ItemPriceCharts.Services.Models.OnlineShop", "OnlineShop")
                        .WithMany("Items")
                        .HasForeignKey("OnlineShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OnlineShop");
                });

            modelBuilder.Entity("OnlineShopUserAccount", b =>
                {
                    b.HasOne("ItemPriceCharts.Services.Models.OnlineShop", null)
                        .WithMany()
                        .HasForeignKey("OnlineShopsForAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ItemPriceCharts.Services.Models.UserAccount", null)
                        .WithMany()
                        .HasForeignKey("UserAccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ItemPriceCharts.Services.Models.OnlineShop", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}