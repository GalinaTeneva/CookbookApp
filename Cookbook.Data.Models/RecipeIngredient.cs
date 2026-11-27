using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Cookbook.Data.Common.EntityConstants.RecipeIngredientEntity;
using static Cookbook.Common.ErrorMessages.RecipeIngredient;

namespace Cookbook.Data.Models
{
    [Comment("Associates recipes with ingredients and stores the quantity of each ingredient used.")]
    public class RecipeIngredient
    {
        [Required(ErrorMessage = QuantityRequiredMessage)]
        [MaxLength(IngredientQuantityMaxLength, ErrorMessage = QuantityMaxLengthMessage)]
        public string Quantity { get; set; } = null!;

        [Required(ErrorMessage = RecipeRequiredMessage)]
        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } = null!;

        [Required(ErrorMessage = IngredientRequiredMessage)]
        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; } = null!;

    }
}
