using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cookbook.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlannedMealEntityAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                comment: "Associates recipes with ingredients and stores the quantity of each ingredient used.");

            migrationBuilder.AlterTable(
                name: "Recipes",
                comment: "Stores users' personal recipes.");

            migrationBuilder.AlterTable(
                name: "Ingredients",
                comment: "Stores ingredients used for recipes.");

            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "Stores categories used to classify recipes.");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Recipes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PlannedMeal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlannedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedMeal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedMeal_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlannedMeal_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores recipes scheduled for specific dates.");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4dd95024-058e-44aa-a548-8d28b38a6e41",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07e472fc-414e-41b5-bc7a-ac8311bd3d4c", "AQAAAAIAAYagAAAAEK0kDhOdnphQFSvKSvPYdGrHr7ZCA75uuLyeG1pNwKWb3H2I8Hd5xNNagTlm3PjijQ==", "618536a2-0e39-49aa-add1-5fdd9a337ff5" });

            migrationBuilder.InsertData(
                table: "PlannedMeal",
                columns: new[] { "Id", "Notes", "PlannedDate", "RecipeId", "UserId" },
                values: new object[,]
                {
                    { 1, "Weekend pancake breakfast.", new DateOnly(2026, 10, 28), 1, "4dd95024-058e-44aa-a548-8d28b38a6e41" },
                    { 2, "Serve the pasta with a fresh salad.", new DateOnly(2026, 10, 28), 2, "4dd95024-058e-44aa-a548-8d28b38a6e41" },
                    { 3, "Serve the apple crumble warm.", new DateOnly(2026, 10, 28), 3, "4dd95024-058e-44aa-a548-8d28b38a6e41" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMeal_RecipeId",
                table: "PlannedMeal",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMeal_UserId_PlannedDate_RecipeId",
                table: "PlannedMeal",
                columns: new[] { "UserId", "PlannedDate", "RecipeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedMeal");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.AlterTable(
                name: "RecipesIngredients",
                oldComment: "Associates recipes with ingredients and stores the quantity of each ingredient used.");

            migrationBuilder.AlterTable(
                name: "Recipes",
                oldComment: "Stores users' personal recipes.");

            migrationBuilder.AlterTable(
                name: "Ingredients",
                oldComment: "Stores ingredients used for recipes.");

            migrationBuilder.AlterTable(
                name: "Categories",
                oldComment: "Stores categories used to classify recipes.");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4dd95024-058e-44aa-a548-8d28b38a6e41",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0b0d1db-8486-407e-8d0d-d34ee91266da", "AQAAAAIAAYagAAAAEH0v2iVcCj45ycokDBky/wFA+fiumWbdazvE+WtpKR+hDV5btJE8zDsjVCnQmI+PaA==", "4aae1f3e-1357-4c90-ab2b-70d0fa5cfec3" });
        }
    }
}
