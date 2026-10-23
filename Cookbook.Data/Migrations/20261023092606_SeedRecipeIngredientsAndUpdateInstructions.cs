using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cookbook.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRecipeIngredientsAndUpdateInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "RecipesIngredients",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4dd95024-058e-44aa-a548-8d28b38a6e41",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0b0d1db-8486-407e-8d0d-d34ee91266da", "AQAAAAIAAYagAAAAEH0v2iVcCj45ycokDBky/wFA+fiumWbdazvE+WtpKR+hDV5btJE8zDsjVCnQmI+PaA==", "4aae1f3e-1357-4c90-ab2b-70d0fa5cfec3" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Flour" },
                    { 2, "Baking powder" },
                    { 3, "Sugar" },
                    { 4, "Salt" },
                    { 5, "Milk" },
                    { 6, "Eggs" },
                    { 7, "Butter" },
                    { 8, "Pasta" },
                    { 9, "Olive oil" },
                    { 10, "Garlic" },
                    { 11, "Chopped tomatoes" },
                    { 12, "Black pepper" },
                    { 13, "Fresh basil" },
                    { 14, "Parmesan" },
                    { 15, "Apples" },
                    { 16, "Cinnamon" }
                });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "* Mix 200 g flour, 2 teaspoons baking powder, 1 tablespoon sugar, and a pinch of salt.\n* Whisk 250 ml milk, 1 egg, and 2 tablespoons melted butter.\n* Stir the wet ingredients into the dry ingredients until just combined.\n* Heat a lightly greased frying pan over medium heat.\n* Add small portions of batter. Cook until bubbles appear, then flip and cook until golden.\n* Serve warm." });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "* Cook 250 g pasta in salted boiling water according to the package instructions.\n* Heat 2 tablespoons olive oil in a saucepan. Add 2 minced garlic cloves and cook for 30 seconds.\n* Add 400 g chopped tomatoes and simmer for 15 minutes.\n* Season with salt and black pepper, then stir in a handful of fresh basil.\n* Drain the pasta, reserving a little cooking water.\n* Toss with the sauce, adding cooking water if needed. Serve with grated Parmesan." });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "* Preheat the oven to 180 C.\n* Peel, core, and slice 4 apples. Toss with 2 tablespoons sugar and 1 teaspoon cinnamon.\n* Transfer the apples to a baking dish.\n* Rub 100 g cold butter into 150 g flour until the mixture resembles coarse crumbs.\n* Stir in 75 g sugar and sprinkle over the apples.\n* Bake for 35 to 40 minutes until golden and the apples are tender." });

            migrationBuilder.InsertData(
                table: "RecipesIngredients",
                columns: new[] { "IngredientId", "RecipeId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "200 g" },
                    { 2, 1, "2 teaspoons" },
                    { 3, 1, "1 tablespoon" },
                    { 4, 1, "A pinch" },
                    { 5, 1, "250 ml" },
                    { 6, 1, "1" },
                    { 7, 1, "2 tablespoons, melted; extra for greasing" },
                    { 4, 2, "To taste" },
                    { 8, 2, "250 g" },
                    { 9, 2, "2 tablespoons" },
                    { 10, 2, "2 cloves" },
                    { 11, 2, "400 g" },
                    { 12, 2, "To taste" },
                    { 13, 2, "1 handful" },
                    { 14, 2, "To serve" },
                    { 1, 3, "150 g" },
                    { 3, 3, "2 tablespoons for filling; 75 g for topping" },
                    { 7, 3, "100 g, cold" },
                    { 15, 3, "4" },
                    { 16, 3, "1 teaspoon" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 14, 2 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 15, 3 });

            migrationBuilder.DeleteData(
                table: "RecipesIngredients",
                keyColumns: new[] { "IngredientId", "RecipeId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "RecipesIngredients",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4dd95024-058e-44aa-a548-8d28b38a6e41",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9a587e6f-3a24-4c68-9d2c-a77ed0f8b313", "AQAAAAIAAYagAAAAEKClPdcowQNkibCFbrUMFCJFxvQYH5t6GxPq75hAOb4+HAX2dxmFerF/GwAH0LwDpg==", "57b5e447-44e7-41b0-ab08-e8104752939f" });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 23, 0, 27, 15, 978, DateTimeKind.Local).AddTicks(8527), "1. Mix 200 g flour, 2 teaspoons baking powder, 1 tablespoon sugar, and a pinch of salt.\n2. Whisk 250 ml milk, 1 egg, and 2 tablespoons melted butter.\n3. Stir the wet ingredients into the dry ingredients until just combined.\n4. Heat a lightly greased frying pan over medium heat.\n5. Add small portions of batter. Cook until bubbles appear, then flip and cook until golden.\n6. Serve warm with fresh fruit or maple syrup." });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 23, 0, 27, 16, 63, DateTimeKind.Local).AddTicks(1556), "1. Cook 250 g pasta in salted boiling water according to the package instructions.\n2. Heat 2 tablespoons olive oil in a saucepan. Add 2 minced garlic cloves and cook for 30 seconds.\n3. Add 400 g chopped tomatoes and simmer for 15 minutes.\n4. Season with salt and black pepper, then stir in a handful of fresh basil.\n5. Drain the pasta, reserving a little cooking water.\n6. Toss with the sauce, adding cooking water if needed. Serve with grated Parmesan." });

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "Instructions" },
                values: new object[] { new DateTime(2026, 10, 23, 0, 27, 16, 63, DateTimeKind.Local).AddTicks(1645), "1. Preheat the oven to 180 C.\n2. Peel, core, and slice 4 apples. Toss with 2 tablespoons sugar and 1 teaspoon cinnamon.\n3. Transfer the apples to a baking dish.\n4. Rub 100 g cold butter into 150 g flour until the mixture resembles coarse crumbs.\n5. Stir in 75 g sugar and sprinkle over the apples.\n6. Bake for 35 to 40 minutes until golden and the apples are tender." });
        }
    }
}
