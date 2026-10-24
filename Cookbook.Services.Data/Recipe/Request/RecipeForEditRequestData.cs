using Cookbook.Services.Data.Ingredient.Response;
using Cookbook.Services.Data.RecipeIngredient;

namespace Cookbook.Services.Data.Recipe.Request
{
    public class RecipeForEditRequestData
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Instructions { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public IEnumerable<RecipeIngredientRequestData> RecipeIngredients { get; set; } = new List<RecipeIngredientRequestData>();

        public int CategoryId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
