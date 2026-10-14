using System.ComponentModel.DataAnnotations;

using static CookbookApp.Common.EntityValidation.IngredientEntityValidation;

namespace CookbookApp.Data.Models
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
