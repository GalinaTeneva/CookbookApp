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
                    "* Mix 200 g flour, 2 teaspoons baking powder, " +
                    "1 tablespoon sugar, and a pinch of salt.\n" +
                    "* Whisk 250 ml milk, 1 egg, and 2 tablespoons melted butter.\n" +
                    "* Stir the wet ingredients into the dry ingredients " +
                    "until just combined.\n" +
                    "* Heat a lightly greased frying pan over medium heat.\n" +
                    "* Add small portions of batter. Cook until bubbles appear, " +
                    "then flip and cook until golden.\n" +
                    "* Serve warm.",
                ImageUrl = null,
                CreatedOn = new DateTime(2026, 10, 10),
                CategoryId = 1, // Breakfast
                UserId = UserConfiguration.SeedUser.Id
            },
            new Recipe
            {
                Id = 2,
                Title = "Tomato and Basil Pasta",
                Description = "A comforting pasta dish with a simple homemade tomato sauce.",
                Instructions =
                    "* Cook 250 g pasta in salted boiling water " +
                    "according to the package instructions.\n" +
                    "* Heat 2 tablespoons olive oil in a saucepan. " +
                    "Add 2 minced garlic cloves and cook for 30 seconds.\n" +
                    "* Add 400 g chopped tomatoes and simmer for 15 minutes.\n" +
                    "* Season with salt and black pepper, then stir in " +
                    "a handful of fresh basil.\n" +
                    "* Drain the pasta, reserving a little cooking water.\n" +
                    "* Toss with the sauce, adding cooking water if needed. " +
                    "Serve with grated Parmesan.",
                ImageUrl = null,
                CreatedOn = new DateTime(2026, 10, 16),
                CategoryId = 2, // Lunch
                UserId = UserConfiguration.SeedUser.Id
            },
            new Recipe
            {
                Id = 3,
                Title = "Classic Apple Crumble",
                Description = "Tender cinnamon apples beneath a crisp, buttery topping.",
                Instructions =
                    "* Preheat the oven to 180 C.\n" +
                    "* Peel, core, and slice 4 apples. Toss with " +
                    "2 tablespoons sugar and 1 teaspoon cinnamon.\n" +
                    "* Transfer the apples to a baking dish.\n" +
                    "* Rub 100 g cold butter into 150 g flour " +
                    "until the mixture resembles coarse crumbs.\n" +
                    "* Stir in 75 g sugar and sprinkle over the apples.\n" +
                    "* Bake for 35 to 40 minutes until golden " +
                    "and the apples are tender.",
                ImageUrl = null,
                CreatedOn = new DateTime(2026, 10, 16),
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
