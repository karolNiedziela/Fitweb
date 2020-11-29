using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartOfBody = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Calories = table.Column<double>(type: "float", nullable: false),
                    Proteins = table.Column<double>(type: "float", nullable: false),
                    Carbohydrates = table.Column<double>(type: "float", nullable: false),
                    Fats = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "date", nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DietGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCalories = table.Column<double>(type: "float", nullable: false),
                    Proteins = table.Column<double>(type: "float", nullable: false),
                    Carbohydrates = table.Column<double>(type: "float", nullable: false),
                    Fats = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietGoals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ExerciseId = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    NumberOfSets = table.Column<int>(type: "int", nullable: false),
                    NumberOfReps = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(nullable: true),
                    AddedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExercises_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExercises_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Calories = table.Column<double>(type: "float", nullable: false),
                    Proteins = table.Column<double>(type: "float", nullable: false),
                    Carbohydrates = table.Column<double>(type: "float", nullable: false),
                    Fats = table.Column<double>(type: "float", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Days",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Monday" },
                    { 1, "Tuesday" },
                    { 2, "Wendnesday" },
                    { 3, "Thursday" },
                    { 4, "Friday" },
                    { 5, "Saturday" },
                    { 6, "Sunday" }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Name", "PartOfBody" },
                values: new object[,]
                {
                    { 18, "Kneeling triceps extension", "Triceps" },
                    { 17, "Prone triceps extension", "Triceps" },
                    { 15, "Lying biceps curl", "Biceps" },
                    { 14, "Chin-up", "Biceps" },
                    { 13, "Biceps curl", "Biceps" },
                    { 12, "Squat", "Legs" },
                    { 10, "Jumping squat", "Legs" },
                    { 9, "Incline bench press", "Chest" },
                    { 16, "Triceps extension", "Triceps" },
                    { 7, "Lateral pulldown", "Back" },
                    { 6, "Back fly", "Back" },
                    { 5, "Pull-up", "Back" },
                    { 4, "Trunk Rotation", "Abs" },
                    { 3, "Resisted Crunch", "Abs" },
                    { 2, "Side Plank", "Abs" },
                    { 1, "Crunch", "Abs" },
                    { 8, "Parallel bar dips", "Chest" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Calories", "Carbohydrates", "Fats", "Name", "Proteins" },
                values: new object[,]
                {
                    { 17, 391.0, 0.10000000000000001, 31.699999999999999, "Cheddar cheese", 27.100000000000001 },
                    { 16, 291.0, 0.69999999999999996, 18.699999999999999, "Roasted pork loin", 30.399999999999999 },
                    { 15, 14.0, 2.8999999999999999, 0.20000000000000001, "Lettuce", 1.3999999999999999 },
                    { 14, 540.0, 0.90000000000000002, 50.600000000000001, "Salami", 21.899999999999999 },
                    { 13, 344.0, 78.900000000000006, 0.69999999999999996, "Rice", 6.7000000000000002 },
                    { 12, 17.0, 2.6000000000000001, 0.40000000000000002, "Mushrooms", 2.7000000000000002 },
                    { 11, 15.0, 3.6000000000000001, 0.20000000000000001, "Tomato", 0.90000000000000002 },
                    { 10, 93.0, 0.0, 1.2, "Chicken breast sirloin", 20.399999999999999 },
                    { 6, 92.0, 1.0, 0.0, "Tiger shrimp", 22.0 },
                    { 8, 201.0, 0.0, 13.6, "Raw salmon", 19.899999999999999 },
                    { 7, 162.0, 0.0, 8.4000000000000004, "Smoked salmon", 21.5 },
                    { 5, 36.0, 9.5, 0.29999999999999999, "Lemon", 0.80000000000000004 },
                    { 4, 380.0, 98.0, 0.0, "Brown sugar", 0.10000000000000001 },
                    { 3, 405.0, 99.799999999999997, 0.0, "Sugar", 0.0 },
                    { 2, 46.0, 11.9, 0.20000000000000001, "Peach", 1.0 },
                    { 1, 95.0, 23.5, 0.29999999999999999, "Banana", 1.0 },
                    { 9, 105.0, 0.10000000000000001, 4.5999999999999996, "Turkey tenderloin", 15.800000000000001 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Admin" },
                    { 1, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietGoals_UserId",
                table: "DietGoals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_DayId",
                table: "UserExercises",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_ExerciseId",
                table: "UserExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_UserId",
                table: "UserExercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_ProductId",
                table: "UserProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietGoals");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "UserProducts");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
