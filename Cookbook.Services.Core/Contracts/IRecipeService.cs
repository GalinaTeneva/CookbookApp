using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;

namespace Cookbook.Services.Core.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData);

        Task<bool> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData);
        
        Task<GetRecipeDetailsResponseData?> GetRecipeDetailsAsync(RecipeAccessRequestData accessRequestData);

        Task<bool> IsUserCreatorAsync(RecipeAccessRequestData accessRequestData);

        Task<GetRecipeForEditResponseData?> GetRecipeForEditAsync(RecipeAccessRequestData accessRequestData);

        Task<bool> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData);

        Task<DeleteRecipeResponseData> GetRecipeDeleteDetailsAsync(RecipeAccessRequestData accessRequestData);

        Task<bool> DeleteRecipeAsync(RecipeAccessRequestData accessRequestData);
    }
}
