using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class NewSystemParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                                INSERT INTO SystemParameters(Name, Value)
                                VALUES 
                                    ('BonusDaysOnCreate', 7.00),
                                    ('BonusTransfersPerMonth', 1.00);
                                ";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
