using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class RemovePrecisionFromProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Proteins",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Fats",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Carbohydrates",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(2)",
                oldPrecision: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Proteins",
                table: "Products",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Fats",
                table: "Products",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Carbohydrates",
                table: "Products",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "Products",
                type: "float(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
