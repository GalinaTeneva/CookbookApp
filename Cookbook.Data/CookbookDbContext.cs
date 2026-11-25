using Cookbook.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Data
{
    public class CookbookDbContext : IdentityDbContext
    {
        public CookbookDbContext(DbContextOptions<CookbookDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeIngredient> RecipesIngredients { get; set; }
        public virtual DbSet<PlannedMeal> PlannedMeal {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(r => new { r.RecipeId, r.IngredientId });

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Category)
                .WithMany(r => r.Recipes)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                        
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannedMeal>()
                .HasOne(pm => pm.Recipe)
                .WithMany(pm => pm.PlannedMeals)
                .HasForeignKey(pm => pm.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannedMeal>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlannedMeal>()
                .HasIndex(pm => new { pm.UserId, pm.PlannedDate, pm.RecipeId })
                .IsUnique();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookbookDbContext).Assembly);
        }
    }
}
