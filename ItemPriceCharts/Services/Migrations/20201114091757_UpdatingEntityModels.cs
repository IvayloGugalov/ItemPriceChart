using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Services.Migrations
{
    public partial class UpdatingEntityModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemPrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PriceDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlineShop",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    URL = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineShop", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: false),
                    CurrentPrice = table.Column<double>(nullable: false),
                    URL = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    OnlineShopId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_OnlineShop_OnlineShopId",
                        column: x => x.OnlineShopId,
                        principalTable: "OnlineShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_OnlineShopId",
                table: "Item",
                column: "OnlineShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "ItemPrice");

            migrationBuilder.DropTable(
                name: "OnlineShop");
        }
    }
}
