using Cookbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public static IEnumerable<Recipe> SeedRecipes = new List<Recipe>()
        {
            new Recipe
            {
                Id = 1,
                Title = "Fluffy Homemade Pancakes",
                Description = "Golden, fluffy pancakes for a relaxed weekend breakfast.",
                Instructions =
                    "1. Mix 200 g flour, 2 teaspoons baking powder, " +
                    "1 tablespoon sugar, and a pinch of salt.\n" +
                    "2. Whisk 250 ml milk, 1 egg, and 2 tablespoons melted butter.\n" +
                    "3. Stir the wet ingredients into the dry ingredients " +
                    "until just combined.\n" +
                    "4. Heat a lightly greased frying pan over medium heat.\n" +
                    "5. Add small portions of batter. Cook until bubbles appear, " +
                    "then flip and cook until golden.\n" +
                    "6. Serve warm with fresh fruit or maple syrup.",
                ImageUrl = null,
                CreatedOn = DateTime.Now,
                CategoryId = 1, // Breakfast
                UserId = UserConfiguration.SeedUser.Id
            },
            new Recipe
            {
                Id = 2,
                Title = "Tomato and Basil Pasta",
                Description = "A comforting pasta dish with a simple homemade tomato sauce.",
                Instructions =
                    "1. Cook 250 g pasta in salted boiling water " +
                    "according to the package instructions.\n" +
                    "2. Heat 2 tablespoons olive oil in a saucepan. " +
                    "Add 2 minced garlic cloves and cook for 30 seconds.\n" +
                    "3. Add 400 g chopped tomatoes and simmer for 15 minutes.\n" +
                    "4. Season with salt and black pepper, then stir in " +
                    "a handful of fresh basil.\n" +
                    "5. Drain the pasta, reserving a little cooking water.\n" +
                    "6. Toss with the sauce, adding cooking water if needed. " +
                    "Serve with grated Parmesan.",
                ImageUrl = null,
                CreatedOn = DateTime.Now,
                CategoryId = 2, // Lunch
                UserId = UserConfiguration.SeedUser.Id
            },
            new Recipe
            {
                Id = 3,
                Title = "Classic Apple Crumble",
                Description = "Tender cinnamon apples beneath a crisp, buttery topping.",
                Instructions =
                    "1. Preheat the oven to 180 C.\n" +
                    "2. Peel, core, and slice 4 apples. Toss with " +
                    "2 tablespoons sugar and 1 teaspoon cinnamon.\n" +
                    "3. Transfer the apples to a baking dish.\n" +
                    "4. Rub 100 g cold butter into 150 g flour " +
                    "until the mixture resembles coarse crumbs.\n" +
                    "5. Stir in 75 g sugar and sprinkle over the apples.\n" +
                    "6. Bake for 35 to 40 minutes until golden " +
                    "and the apples are tender.",
                ImageUrl = null,
                CreatedOn = DateTime.Now,
                CategoryId = 4, // Desserts
                UserId = UserConfiguration.SeedUser.Id
            }
        };

        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasData(SeedRecipes);
        }
    }
}
