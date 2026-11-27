using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Cookbook.Data.Common.EntityConstants.RecipeEntity;
using static Cookbook.Common.ErrorMessages.Recipe;

namespace Cookbook.Data.Models
{
    [Comment("Stores users' personal recipes.")]
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = TitleRequiredMessage)]
        [MaxLength(TitleMaxLength, ErrorMessage = TitleMaxLengthMessage)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = InstructionsRequiredMessage)]
        [MaxLength(InstructionsMaxLength, ErrorMessage = InstructionsMaxLengthMessage)]
        public string Instructions { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = CreatedOnRequiredMessage)]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = CategoryRequiredMessage)]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [Required(ErrorMessage = UserRequiredMessage)]
        [ForeignKey(nameof(CreatedBy))]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser CreatedBy { get; set; } = null!;

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public virtual ICollection<PlannedMeal> PlannedMeals { get; set; } = new List<PlannedMeal>();
    }
}
