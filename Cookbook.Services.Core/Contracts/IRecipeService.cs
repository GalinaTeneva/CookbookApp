using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;

namespace Cookbook.Services.Core.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData);

        Task<bool> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData);
    }
}
