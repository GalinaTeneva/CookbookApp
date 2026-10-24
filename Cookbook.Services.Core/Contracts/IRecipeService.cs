using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;

namespace Cookbook.Services.Core.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData);

        Task<bool> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData);

        //TODO: Use RecipeAccessRequestData in the method + check the ownership in the service method implementation
        Task<GetRecipeDetailsResponseData?> GetRecipeDetailsAsync(int recipeId);

        Task<bool> IsUserCreatorAsync(RecipeAccessRequestData accessRequestData);

        Task<GetRecipeForEditResponseData?> GetRecipeForEditAsync(RecipeAccessRequestData accessRequestData);

        Task<bool> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData);
    }
}
