using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cookbook.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4dd95024-058e-44aa-a548-8d28b38a6e41", 0, "9a587e6f-3a24-4c68-9d2c-a77ed0f8b313", "test@gmail.com", true, false, null, "TEST@GMAIL.COM", "TEST@GMAIL.COM", "AQAAAAIAAYagAAAAEKClPdcowQNkibCFbrUMFCJFxvQYH5t6GxPq75hAOb4+HAX2dxmFerF/GwAH0LwDpg==", null, false, "57b5e447-44e7-41b0-ab08-e8104752939f", false, "test@gmail.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Breakfast" },
                    { 2, "Lunch" },
                    { 3, "Dinner" },
                    { 4, "Desserts" },
                    { 5, "Salads" },
                    { 6, "Soups" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CategoryId", "CreatedOn", "Description", "ImageUrl", "Instructions", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 23, 0, 27, 15, 978, DateTimeKind.Local).AddTicks(8527), "Golden, fluffy pancakes for a relaxed weekend breakfast.", null, "1. Mix 200 g flour, 2 teaspoons baking powder, 1 tablespoon sugar, and a pinch of salt.\n2. Whisk 250 ml milk, 1 egg, and 2 tablespoons melted butter.\n3. Stir the wet ingredients into the dry ingredients until just combined.\n4. Heat a lightly greased frying pan over medium heat.\n5. Add small portions of batter. Cook until bubbles appear, then flip and cook until golden.\n6. Serve warm with fresh fruit or maple syrup.", "Fluffy Homemade Pancakes", "4dd95024-058e-44aa-a548-8d28b38a6e41" },
                    { 2, 2, new DateTime(2026, 10, 23, 0, 27, 16, 63, DateTimeKind.Local).AddTicks(1556), "A comforting pasta dish with a simple homemade tomato sauce.", null, "1. Cook 250 g pasta in salted boiling water according to the package instructions.\n2. Heat 2 tablespoons olive oil in a saucepan. Add 2 minced garlic cloves and cook for 30 seconds.\n3. Add 400 g chopped tomatoes and simmer for 15 minutes.\n4. Season with salt and black pepper, then stir in a handful of fresh basil.\n5. Drain the pasta, reserving a little cooking water.\n6. Toss with the sauce, adding cooking water if needed. Serve with grated Parmesan.", "Tomato and Basil Pasta", "4dd95024-058e-44aa-a548-8d28b38a6e41" },
                    { 3, 4, new DateTime(2026, 10, 23, 0, 27, 16, 63, DateTimeKind.Local).AddTicks(1645), "Tender cinnamon apples beneath a crisp, buttery topping.", null, "1. Preheat the oven to 180 C.\n2. Peel, core, and slice 4 apples. Toss with 2 tablespoons sugar and 1 teaspoon cinnamon.\n3. Transfer the apples to a baking dish.\n4. Rub 100 g cold butter into 150 g flour until the mixture resembles coarse crumbs.\n5. Stir in 75 g sugar and sprinkle over the apples.\n6. Bake for 35 to 40 minutes until golden and the apples are tender.", "Classic Apple Crumble", "4dd95024-058e-44aa-a548-8d28b38a6e41" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4dd95024-058e-44aa-a548-8d28b38a6e41");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
