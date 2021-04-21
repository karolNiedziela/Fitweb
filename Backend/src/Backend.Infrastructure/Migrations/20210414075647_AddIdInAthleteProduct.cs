using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class AddIdInAthleteProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AthleteProducts",
                table: "AthleteProducts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AthleteProducts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AthleteProducts",
                table: "AthleteProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteProducts_AthleteId",
                table: "AthleteProducts",
                column: "AthleteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AthleteProducts",
                table: "AthleteProducts");

            migrationBuilder.DropIndex(
                name: "IX_AthleteProducts_AthleteId",
                table: "AthleteProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AthleteProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AthleteProducts",
                table: "AthleteProducts",
                columns: new[] { "AthleteId", "ProductId" });
        }
    }
}
