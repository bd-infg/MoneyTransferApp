using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class AdminAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                                INSERT INTO Accounts(Id, Bank, Balance, OpeningDate, LastTransactionDate, MonthlyIncome, MonthlyOutcome, Password, Blocked)
                                VALUES 
                                    ('0000000000000', 0, 0, '2000-01-01 00:00:00.000', '2000-01-01 00:00:00.000', 0, 0, 'adminX', 0);
                                ";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
