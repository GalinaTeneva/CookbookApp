using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Cookbook.Common.EntityValidation.RecipeEntityValidation;

namespace Cookbook.Data.Models
{
    [Comment("Stores users' personal recipes.")]
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(InstructionsMaxLength)]
        public string Instructions { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CreatedBy))]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser CreatedBy { get; set; } = null!;

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public virtual ICollection<PlannedMeal> PlannedMeals { get; set; } = new List<PlannedMeal>();
    }
}
