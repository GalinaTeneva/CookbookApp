using Cookbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    internal class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public static IEnumerable<RecipeIngredient> SeededRecipesIngredients = new List<RecipeIngredient>
        {
            // Recipe 1: Fluffy Homemade Pancakes
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 1,
                    Quantity = "200 g"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 2,
                    Quantity = "2 teaspoons"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 3,
                    Quantity = "1 tablespoon"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 4,
                    Quantity = "A pinch"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 5,
                    Quantity = "250 ml"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 6,
                    Quantity = "1"
                },
                new RecipeIngredient
                {
                    RecipeId = 1,
                    IngredientId = 7,
                    Quantity = "2 tablespoons, melted; extra for greasing"
                },

                // Recipe 2: Tomato and Basil Pasta
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 8,
                    Quantity = "250 g"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 9,
                    Quantity = "2 tablespoons"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 10,
                    Quantity = "2 cloves"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 11,
                    Quantity = "400 g"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 4,
                    Quantity = "To taste"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 12,
                    Quantity = "To taste"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 13,
                    Quantity = "1 handful"
                },
                new RecipeIngredient
                {
                    RecipeId = 2,
                    IngredientId = 14,
                    Quantity = "To serve"
                },

                // Recipe 3: Classic Apple Crumble
                new RecipeIngredient
                {
                    RecipeId = 3,
                    IngredientId = 15,
                    Quantity = "4"
                },
                new RecipeIngredient
                {
                    RecipeId = 3,
                    IngredientId = 3,
                    Quantity = "2 tablespoons for filling; 75 g for topping"
                },
                new RecipeIngredient
                {
                    RecipeId = 3,
                    IngredientId = 16,
                    Quantity = "1 teaspoon"
                },
                new RecipeIngredient
                {
                    RecipeId = 3,
                    IngredientId = 7,
                    Quantity = "100 g, cold"
                },
                new RecipeIngredient
                {
                    RecipeId = 3,
                    IngredientId = 1,
                    Quantity = "150 g"
                }
        };

        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.HasData(SeededRecipesIngredients);
        }
    }
}
