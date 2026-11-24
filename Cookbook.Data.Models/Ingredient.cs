using System.ComponentModel.DataAnnotations;

using static Cookbook.Common.EntityValidation.IngredientEntityValidation;

namespace Cookbook.Data.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        //TODO: Consider making the name unique
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
