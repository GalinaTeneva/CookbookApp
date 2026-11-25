using Cookbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    public class PlannedMealConfiguration : IEntityTypeConfiguration<PlannedMeal>
    {
        public static readonly IEnumerable<PlannedMeal> SeededPlannedMeals = new List<PlannedMeal>
        {
            new PlannedMeal
            {
                Id = 1,
                RecipeId = 1,
                UserId = UserConfiguration.SeedUser.Id,
                PlannedDate = new DateOnly(2026, 10, 28),
                Notes = "Weekend pancake breakfast."
            },
            new PlannedMeal
            {
                Id = 2,
                RecipeId = 2,
                UserId = UserConfiguration.SeedUser.Id,
                PlannedDate = new DateOnly(2026, 10, 28),
                Notes = "Serve the pasta with a fresh salad."
            },
            new PlannedMeal
            {
                Id = 3,
                RecipeId = 3,
                UserId = UserConfiguration.SeedUser.Id,
                PlannedDate = new DateOnly(2026, 10, 28),
                Notes = "Serve the apple crumble warm."
            }
        };

        public void Configure(EntityTypeBuilder<PlannedMeal> builder)
        {
            builder.HasData(SeededPlannedMeals);
        }
    }
}
