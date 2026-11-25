using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Cookbook.Data.Common.EntityConstants.PlannedMealEntity;

namespace Cookbook.Data.Models
{
    [Comment("Stores recipes scheduled for specific dates.")]
    public class PlannedMeal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CreatedBy))]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser CreatedBy { get; set; } = null!; 

        [Required]
        [Column(TypeName = PlannedDateColumnType)]
        public DateOnly PlannedDate { get; set; }

        public string? Notes { get; set; }
    }
}
