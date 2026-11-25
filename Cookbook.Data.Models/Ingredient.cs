using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using static Cookbook.Data.Common.EntityConstants.IngredientEntity;

namespace Cookbook.Data.Models
{
    [Comment("Stores ingredients used for recipes.")]
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
