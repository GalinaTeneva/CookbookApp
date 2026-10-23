using Cookbook.Services.Data.Ingredient.Response;

namespace Cookbook.Services.Data.Recipe.Response
{
    public class GetRecipeDetailsResponseData
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public IEnumerable<string> InstructionSteps { get; set; } = new List<string>();

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public IEnumerable<RecipeIngredientsResponseData> RecipeIngredients { get; set; } = new List<RecipeIngredientsResponseData>();
    }
}
