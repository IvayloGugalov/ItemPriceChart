using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineShops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineShops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 220, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentPrice_Price = table.Column<double>(type: "REAL", nullable: true),
                    OriginalPrice_Price = table.Column<double>(type: "REAL", nullable: true),
                    URL = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    OnlineShopId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_OnlineShops_OnlineShopId",
                        column: x => x.OnlineShopId,
                        principalTable: "OnlineShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountOnlineShops",
                columns: table => new
                {
                    UserAccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OnlineShopId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountOnlineShops", x => new { x.UserAccountId, x.OnlineShopId });
                    table.ForeignKey(
                        name: "FK_UserAccountOnlineShops_OnlineShops_OnlineShopId",
                        column: x => x.OnlineShopId,
                        principalTable: "OnlineShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccountOnlineShops_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items_PricesForItem",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items_PricesForItem", x => new { x.ItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_Items_PricesForItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_OnlineShopId",
                table: "Items",
                column: "OnlineShopId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Username_Email",
                table: "UserAccount",
                columns: new[] { "Username", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountOnlineShops_OnlineShopId",
                table: "UserAccountOnlineShops",
                column: "OnlineShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items_PricesForItem");

            migrationBuilder.DropTable(
                name: "UserAccountOnlineShops");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "OnlineShops");
        }
    }
}
