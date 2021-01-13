using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class RemoveCalvesFromPartOfBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Biceps");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Triceps");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Shoulders");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Abdominals");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Back");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Forearm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Calves");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Biceps");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Triceps");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Shoulders");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Abdominals");

            migrationBuilder.UpdateData(
                table: "PartOfBodies",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Back");

            migrationBuilder.InsertData(
                table: "PartOfBodies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 9, "Forearm" });
        }
    }
}
