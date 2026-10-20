namespace Cookbook.Web.ViewModels.Recipe
{
    public class MyRecipesViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
    }
}
