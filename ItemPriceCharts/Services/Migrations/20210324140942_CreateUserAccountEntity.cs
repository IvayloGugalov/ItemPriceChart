using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemPriceCharts.Services.Migrations
{
    public partial class CreateUserAccountEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 220, nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlineShopUserAccount",
                columns: table => new
                {
                    OnlineShopsForAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAccountsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineShopUserAccount", x => new { x.OnlineShopsForAccountId, x.UserAccountsId });
                    table.ForeignKey(
                        name: "FK_OnlineShopUserAccount_OnlineShop_OnlineShopsForAccountId",
                        column: x => x.OnlineShopsForAccountId,
                        principalTable: "OnlineShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlineShopUserAccount_UserAccount_UserAccountsId",
                        column: x => x.UserAccountsId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineShop_Id",
                table: "OnlineShop",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrice_Id",
                table: "ItemPrice",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_Id",
                table: "Item",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineShopUserAccount_UserAccountsId",
                table: "OnlineShopUserAccount",
                column: "UserAccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Id",
                table: "UserAccount",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Username_Email",
                table: "UserAccount",
                columns: new[] { "Username", "Email" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
