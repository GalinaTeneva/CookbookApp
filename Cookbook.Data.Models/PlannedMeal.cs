using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Cookbook.Data.Common.EntityConstants.PlannedMealEntity;
using static Cookbook.Common.ErrorMessages.PlannedMeal;

namespace Cookbook.Data.Models
{
    [Comment("Stores recipes scheduled for specific dates.")]
    public class PlannedMeal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = RecipeRequiredMessage)]
        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } = null!;

        [Required(ErrorMessage = UserRequiredMessage)]
        [ForeignKey(nameof(CreatedBy))]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser CreatedBy { get; set; } = null!; 

        [Required(ErrorMessage = PlannedDateRequiredMessage)]
        [Column(TypeName = PlannedDateColumnType)]
        public DateOnly PlannedDate { get; set; }

        public string? Notes { get; set; }
    }
}
