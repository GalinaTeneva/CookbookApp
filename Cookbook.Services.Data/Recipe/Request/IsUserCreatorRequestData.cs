namespace Cookbook.Services.Data.Recipe.Request
{
    public class IsUserCreatorRequestData
    {
        public string UserId { get; set; } = null!;

        public int RecipeId { get; set; }
    }
}
