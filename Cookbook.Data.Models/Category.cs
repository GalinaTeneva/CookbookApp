using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using static Cookbook.Data.Common.EntityConstants.CategoryEntity;

namespace Cookbook.Data.Models
{
    [Comment("Stores categories used to classify recipes.")]
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
