using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Bank = table.Column<int>(type: "int", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    MonthlyOutcome = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    FromAccountId = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    ToAccountId = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Flow = table.Column<byte>(type: "tinyint", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(13)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
