using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class TimeStampOnTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Transactions",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Transactions");
        }
    }
}
