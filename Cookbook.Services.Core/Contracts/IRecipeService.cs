using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;

namespace Cookbook.Services.Core.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData);

        //TODO: Rename to AddRecipeWithIngredients or AddRecipe
        Task<RecipeServiceStatusResponseModel> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData);
        
        Task<GetRecipeDetailsResponseData?> GetRecipeDetailsAsync(RecipeAccessRequestData accessRequestData);

        Task<bool> IsUserCreatorAsync(RecipeAccessRequestData accessRequestData);

        Task<GetRecipeForEditResponseData?> GetRecipeForEditAsync(RecipeAccessRequestData accessRequestData);

        Task<RecipeServiceStatusResponseModel> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData);

        Task<DeleteRecipeResponseData?> GetRecipeDeleteDetailsAsync(RecipeAccessRequestData accessRequestData);

        Task<RecipeServiceStatusResponseModel> DeleteRecipeAsync(RecipeAccessRequestData accessRequestData);
    }
}
