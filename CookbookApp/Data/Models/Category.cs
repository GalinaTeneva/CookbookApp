using System.ComponentModel.DataAnnotations;

using static CookbookApp.Common.EntityValidation.CategoryEntityValidation;

namespace CookbookApp.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
