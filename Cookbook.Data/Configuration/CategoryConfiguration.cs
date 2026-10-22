using Cookbook.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cookbook.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public static IEnumerable<Category> SeedCategories = new List<Category>
        {
            new Category{Id = 1, Name = "Breakfast"},
            new Category{Id = 2, Name = "Lunch"},
            new Category{Id = 3, Name = "Dinner"},
            new Category{Id = 4, Name = "Desserts"},
            new Category{Id = 5, Name = "Salads"},
            new Category{Id = 6, Name = "Soups"},
        };

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(SeedCategories);
        }
    }
}
