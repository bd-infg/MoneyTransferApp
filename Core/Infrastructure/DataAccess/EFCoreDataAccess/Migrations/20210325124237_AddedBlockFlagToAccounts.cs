using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class AddedBlockFlagToAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Blocked",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "Accounts");
        }
    }
}
