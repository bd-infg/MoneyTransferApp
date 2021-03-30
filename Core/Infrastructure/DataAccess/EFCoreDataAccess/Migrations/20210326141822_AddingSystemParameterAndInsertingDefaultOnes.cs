using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDataAccess.Migrations
{
    public partial class AddingSystemParameterAndInsertingDefaultOnes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemParameters", x => x.Id);
                });

            var sp = @"
                                INSERT INTO SystemParameters(Name, Value)
                                VALUES 
                                    ('MonthlyIncomeLimit', 1000000.00),
                                    ('MonthlyOutcomeLimit',1000000.00),
                                    ('ProvisionLimit', 10000.00),
                                    ('ProvisionUnderLimitCost', 100.00),
                                    ('ProvisionOverLimitCostPercent', 1.00);
                                ";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemParameters");
        }
    }
}
