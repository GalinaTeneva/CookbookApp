namespace Cookbook.Services.Data.RecipeIngredient
{
    public class RecipeIngredientRequestData
    {
        public int RecipeId {  get; set; }

        public int IngredientId { get; set; }

        public string IngredientName { get; set; } = null!;

        public string Quantity { get; set; } = null!;

    }
}
