namespace Cookbook.Services.Data.Recipe.Response
{
    public class GetAllUserRecipesResponseData
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}
