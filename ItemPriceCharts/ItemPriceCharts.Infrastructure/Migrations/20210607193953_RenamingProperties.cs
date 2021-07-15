using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Infrastructure.Migrations
{
    public partial class RenamingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                table: "OnlineShops",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Items",
                newName: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "OnlineShops",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Items",
                newName: "URL");
        }
    }
}
