using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static CookbookApp.Common.EntityValidation.IngredientEntityValidation;

namespace CookbookApp.Data.Models
{
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
