using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class CaloricDemand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaloricDemandId",
                table: "Athletes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_CaloricDemandId",
                table: "Athletes",
                column: "CaloricDemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Athletes_CaloricDemand_CaloricDemandId",
                table: "Athletes",
                column: "CaloricDemandId",
                principalTable: "CaloricDemand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_CaloricDemand_CaloricDemandId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_CaloricDemandId",
                table: "Athletes");

            migrationBuilder.DropColumn(
                name: "CaloricDemandId",
                table: "Athletes");
        }
    }
}
