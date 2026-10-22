using Cookbook.Web.ViewModels.Genre;

namespace Cookbook.Web.ViewModels.Recipe
{
    public class AddRecipeInputModel
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Instructions { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; } = null!;

        public string IngredientsText { get; set; } = null!;

        public IEnumerable<AllCategoriesViewModel> Categories { get; set; } = null!;
    }
}
