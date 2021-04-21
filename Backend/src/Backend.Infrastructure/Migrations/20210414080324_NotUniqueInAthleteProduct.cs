using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class NotUniqueInAthleteProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AthleteProducts_AthleteId",
                table: "AthleteProducts");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteProducts_AthleteId_ProductId",
                table: "AthleteProducts",
                columns: new[] { "AthleteId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AthleteProducts_AthleteId_ProductId",
                table: "AthleteProducts");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteProducts_AthleteId",
                table: "AthleteProducts",
                column: "AthleteId");
        }
    }
}
