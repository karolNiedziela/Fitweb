using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class SeedDataOnModelCreating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "PartOfBodyId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(2975), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(3022), "Barbell Chest Press", 1 },
                    { 10, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4166), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4168), "Close-Grip Bench Press", 4 },
                    { 9, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4162), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4164), "Hammer Curl", 3 },
                    { 8, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4157), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4159), "Standinag Dumbbell Curl", 3 },
                    { 7, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4153), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4155), "Leg Extension", 2 },
                    { 11, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4170), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4172), "Cable Rope Triceps Pushdown", 4 },
                    { 5, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4144), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4146), "Squat", 2 },
                    { 4, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4140), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4142), "Push-Ups", 1 },
                    { 3, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4135), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4137), "Wide-Grip Chest Press", 1 },
                    { 2, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4118), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4130), "Dumbbell Chest Press", 1 },
                    { 6, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4148), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(4150), "Leg Curl", 2 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Calories", "Carbohydrates", "CategoryOfProductId", "DateCreated", "DateUpdated", "Fats", "Name", "Proteins" },
                values: new object[,]
                {
                    { 9, 27.0, 8.6999999999999993, 3, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1646), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1648), 0.20000000000000001, "Carrot", 1.0 },
                    { 1, 95.0, 23.5, 2, new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(9527), new DateTime(2021, 5, 17, 16, 59, 22, 475, DateTimeKind.Local).AddTicks(9545), 0.29999999999999999, "Banana", 0.29999999999999999 },
                    { 2, 46.0, 11.9, 2, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1601), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1613), 0.20000000000000001, "Peach", 1.0 },
                    { 3, 54.0, 13.6, 2, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1617), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1619), 0.20000000000000001, "Pineapple", 0.40000000000000002 },
                    { 4, 36.0, 12.1, 2, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1622), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1624), 0.40000000000000002, "Apple", 0.40000000000000002 },
                    { 5, 99.0, 0.0, 1, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1627), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1629), 1.3, "Chicken breast without skin", 21.5 },
                    { 6, 106.0, 0.0, 1, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1632), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1634), 2.7999999999999998, "Veal shoulder", 19.899999999999999 },
                    { 7, 321.0, 0.0, 1, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1636), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1638), 29.300000000000001, "Pork ribs", 15.199999999999999 },
                    { 8, 77.0, 18.300000000000001, 3, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1641), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1643), 0.10000000000000001, "Potatoes", 1.8999999999999999 },
                    { 10, 14.0, 2.8999999999999999, 3, new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1651), new DateTime(2021, 5, 17, 16, 59, 22, 476, DateTimeKind.Local).AddTicks(1653), 0.20000000000000001, "Lettuce", 1.3999999999999999 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
