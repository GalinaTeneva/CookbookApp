using System.ComponentModel.DataAnnotations;

using static Cookbook.Common.EntityValidation.IngredientEntityValidation;

namespace Cookbook.Data.Models
{
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
