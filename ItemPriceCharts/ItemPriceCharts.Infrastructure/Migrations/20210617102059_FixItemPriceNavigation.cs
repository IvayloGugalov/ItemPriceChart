using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Infrastructure.Migrations
{
    public partial class FixItemPriceNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Items_PricesForItem",
                table: "Items_PricesForItem");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Items_PricesForItem",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Items_PricesForItem",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceDateRetrieved",
                table: "Items_PricesForItem",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "OriginalPrice_Price",
                table: "Items",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentPrice_Price",
                table: "Items",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items_PricesForItem",
                table: "Items_PricesForItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_PricesForItem_ItemId",
                table: "Items_PricesForItem",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Items_PricesForItem",
                table: "Items_PricesForItem");

            migrationBuilder.DropIndex(
                name: "IX_Items_PricesForItem_ItemId",
                table: "Items_PricesForItem");

            migrationBuilder.DropColumn(
                name: "PriceDateRetrieved",
                table: "Items_PricesForItem");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Items_PricesForItem",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Items_PricesForItem",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<double>(
                name: "OriginalPrice_Price",
                table: "Items",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentPrice_Price",
                table: "Items",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items_PricesForItem",
                table: "Items_PricesForItem",
                columns: new[] { "ItemId", "Id" });
        }
    }
}
