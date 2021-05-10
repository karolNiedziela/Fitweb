using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class RemovePrecisionFromDietStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalProteins",
                table: "DietStats",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "TotalFats",
                table: "DietStats",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "TotalCarbohydrates",
                table: "DietStats",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "TotalCalories",
                table: "DietStats",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalProteins",
                table: "DietStats",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "TotalFats",
                table: "DietStats",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "TotalCarbohydrates",
                table: "DietStats",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "TotalCalories",
                table: "DietStats",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
