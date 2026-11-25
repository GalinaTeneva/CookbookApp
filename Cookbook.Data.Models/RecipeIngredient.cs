using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Cookbook.Data.Common.EntityConstants.IngredientEntity;

namespace Cookbook.Data.Models
{
    [Comment("Associates recipes with ingredients and stores the quantity of each ingredient used.")]
    public class RecipeIngredient
    {
        [Required]
        [MaxLength(IngredientQuantityMaxLength)]
        public string Quantity { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; } = null!;

    }
}
