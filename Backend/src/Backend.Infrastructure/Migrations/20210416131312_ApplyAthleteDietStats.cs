using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class ApplyAthleteDietStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    TotalProteins = table.Column<double>(type: "float", nullable: false),
                    TotalCarbohydrates = table.Column<double>(type: "float", nullable: false),
                    TotalFats = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AthleteDietStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DietStatId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteDietStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AthleteDietStats_Athletes_UserId",
                        column: x => x.UserId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthleteDietStats_DietStats_DietStatId",
                        column: x => x.DietStatId,
                        principalTable: "DietStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthleteDietStats_DietStatId",
                table: "AthleteDietStats",
                column: "DietStatId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteDietStats_UserId_DietStatId",
                table: "AthleteDietStats",
                columns: new[] { "UserId", "DietStatId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AthleteDietStats");

            migrationBuilder.DropTable(
                name: "DietStats");
        }
    }
}
