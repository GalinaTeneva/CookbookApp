using System.ComponentModel.DataAnnotations;

using static Cookbook.Common.EntityValidation.CategoryEntityValidation;

namespace Cookbook.Data.Models
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
