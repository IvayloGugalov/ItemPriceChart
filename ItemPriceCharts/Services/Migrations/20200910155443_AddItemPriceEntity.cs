using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Services.Migrations
{
    public partial class AddItemPriceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "Entities",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PriceDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPrices_Entities_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrices_ItemId",
                table: "ItemPrices",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPrices");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Entities",
                type: "REAL",
                nullable: true);
        }
    }
}
