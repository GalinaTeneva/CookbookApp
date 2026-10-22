namespace Cookbook.Services.Data.Recipe.Request
{
    public class AddRecipeToUserRecipeListRequestData
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Instructions { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
