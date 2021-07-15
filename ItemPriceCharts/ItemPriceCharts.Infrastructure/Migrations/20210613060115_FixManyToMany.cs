using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Infrastructure.Migrations
{
    public partial class FixManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountOnlineShops_UserAccount_UserAccountId",
                table: "UserAccountOnlineShops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount");

            migrationBuilder.RenameTable(
                name: "UserAccount",
                newName: "UserAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_Username_Email",
                table: "UserAccounts",
                newName: "IX_UserAccounts_Username_Email");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserAccounts_Username_Email",
                table: "UserAccounts",
                columns: new[] { "Username", "Email" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountOnlineShops_UserAccounts_UserAccountId",
                table: "UserAccountOnlineShops",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountOnlineShops_UserAccounts_UserAccountId",
                table: "UserAccountOnlineShops");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserAccounts_Username_Email",
                table: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "UserAccount");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_Username_Email",
                table: "UserAccount",
                newName: "IX_UserAccount_Username_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountOnlineShops_UserAccount_UserAccountId",
                table: "UserAccountOnlineShops",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
