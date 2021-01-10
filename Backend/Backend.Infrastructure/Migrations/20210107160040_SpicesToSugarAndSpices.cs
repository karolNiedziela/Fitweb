using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class SpicesToSugarAndSpices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoriesOfProduct",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Sugar_and_spices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoriesOfProduct",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Spices");
        }
    }
}
