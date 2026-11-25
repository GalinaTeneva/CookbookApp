using Cookbook.Services.Data.Recipe.Enums;

namespace Cookbook.Services.Data.Recipe.Response
{
    public class RecipeServiceStatusResponseModel
    {
        public RecipeServiceResponseStatus Status { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; } = new List<string>();
    }
}
