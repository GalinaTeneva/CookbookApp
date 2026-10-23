using Cookbook.Services.Data.Ingredient.Request;
using Cookbook.Services.Data.Ingredient.Response;

namespace Cookbook.Services.Core.Contracts
{
    public interface IIngredientService
    {
        public Task<IngredientResponseData> FindOrCreateAsync(IngredientRequestData requestData);
    }
}
