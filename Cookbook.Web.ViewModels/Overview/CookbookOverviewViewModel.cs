using Cookbook.Web.ViewModels.Recipe;

namespace Cookbook.Web.ViewModels.Overview
{
    public class CookbookOverviewViewModel
    {
        public int TotalRecipes { get; set; }

        public IEnumerable<CategoryOverviewViewModel> Categories { get; set; } = new List<CategoryOverviewViewModel>();

        public MyRecipesViewModel? LatestRecipe {  get; set; }
    }
}
