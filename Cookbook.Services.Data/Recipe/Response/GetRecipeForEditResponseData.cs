using Cookbook.Services.Data.Ingredient.Response;

namespace Cookbook.Services.Data.Recipe.Response
{
    public class GetRecipeForEditResponseData
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Instructions { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public IEnumerable<RecipeIngredientsResponseData> RecipeIngredients { get; set; } = new List<RecipeIngredientsResponseData>();

        public int CategoryId { get; set; }
    }
}
