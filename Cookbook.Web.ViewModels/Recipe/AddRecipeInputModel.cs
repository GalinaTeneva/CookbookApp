using Cookbook.Web.ViewModels.Genre;
using static Cookbook.Common.EntityValidation.RecipeEntityValidation;
using System.ComponentModel.DataAnnotations;

namespace Cookbook.Web.ViewModels.Recipe
{
    public class AddRecipeInputModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string IngredientsText { get; set; } = null!;

        public int CategoryId { get; set; }

        //public string UserId { get; set; } = null!;

        public IEnumerable<AllCategoriesViewModel> Categories { get; set; } = new List<AllCategoriesViewModel>();
    }
}
