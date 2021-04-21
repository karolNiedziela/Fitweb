using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class OptionalCaloricDemand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Athletes_CaloricDemand_CaloricDemandId",
                table: "Athletes");

            migrationBuilder.DropIndex(
                name: "IX_Athletes_CaloricDemandId",
                table: "Athletes");

            migrationBuilder.AlterColumn<int>(
                name: "CaloricDemandId",
                table: "Athletes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Athletes_CaloricDemandId",
                table: "Athletes",
                column: "CaloricDemandId",
                unique: true,
                filter: "[CaloricDemandId] IS NOT NULL");

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

            migrationBuilder.AlterColumn<int>(
                name: "CaloricDemandId",
                table: "Athletes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
