using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class PartOfBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartOfBody",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "PartOfBodyId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PartOfBodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartOfBodies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PartOfBodies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chest" },
                    { 2, "Legs" },
                    { 3, "Calves" },
                    { 4, "Biceps" },
                    { 5, "Triceps" },
                    { 6, "Shoulders" },
                    { 7, "Abdominals" },
                    { 8, "Back" },
                    { 9, "Forearm" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_PartOfBodyId",
                table: "Exercises",
                column: "PartOfBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_PartOfBodies_PartOfBodyId",
                table: "Exercises",
                column: "PartOfBodyId",
                principalTable: "PartOfBodies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_PartOfBodies_PartOfBodyId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "PartOfBodies");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_PartOfBodyId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "PartOfBodyId",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "PartOfBody",
                table: "Exercises",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");
        }
    }
}
