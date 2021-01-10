using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class DairyToDairyAndEggs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoriesOfProduct",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Dairy_and_eggs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CategoriesOfProduct",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Dairy");
        }
    }
}
