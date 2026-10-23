using Cookbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public static IEnumerable<Ingredient> SeedIngredients = new List<Ingredient>
        {
            new Ingredient { Id = 1, Name = "Flour" },
            new Ingredient { Id = 2, Name = "Baking powder" },
            new Ingredient { Id = 3, Name = "Sugar" },
            new Ingredient { Id = 4, Name = "Salt" },
            new Ingredient { Id = 5, Name = "Milk" },
            new Ingredient { Id = 6, Name = "Eggs" },
            new Ingredient { Id = 7, Name = "Butter" },
            new Ingredient { Id = 8, Name = "Pasta" },
            new Ingredient { Id = 9, Name = "Olive oil" },
            new Ingredient { Id = 10, Name = "Garlic" },
            new Ingredient { Id = 11, Name = "Chopped tomatoes" },
            new Ingredient { Id = 12, Name = "Black pepper" },
            new Ingredient { Id = 13, Name = "Fresh basil" },
            new Ingredient { Id = 14, Name = "Parmesan" },
            new Ingredient { Id = 15, Name = "Apples" },
            new Ingredient { Id = 16, Name = "Cinnamon" }
        };

        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasData(SeedIngredients);
        }
    }
}
