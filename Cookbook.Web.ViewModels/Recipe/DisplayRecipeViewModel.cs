using Cookbook.Web.ViewModels.Ingredient;

namespace Cookbook.Web.ViewModels.Recipe
{
    public class DisplayRecipeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public IEnumerable<string> InstructionSteps { get; set; } = new List<string>();

        public string? ImageUrl { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public IEnumerable<RecipeIngredientsViewModel> RecipeIngredients { get; set; } = new List<RecipeIngredientsViewModel>();
    }
}
