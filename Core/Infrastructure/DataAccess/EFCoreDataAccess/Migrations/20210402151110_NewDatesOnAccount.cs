using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class NewDatesOnAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastIncomeTransactionDate",
                table: "Accounts",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastOutcomeTransactionDate",
                table: "Accounts",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastIncomeTransactionDate",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LastOutcomeTransactionDate",
                table: "Accounts");
        }
    }
}
