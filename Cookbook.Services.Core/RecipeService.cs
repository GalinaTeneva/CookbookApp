using Cookbook.Data;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Services.Core
{
    internal class RecipeService : IRecipeService
    {
        private readonly CookbookDbContext dbContext;

        public RecipeService(CookbookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData)
        {
            if(string.IsNullOrWhiteSpace(requestData.UserId))
            {
                throw new ArgumentException("UserId can not be null or empty", nameof(requestData.UserId));
            }

            IEnumerable<GetAllUserRecipesResponseData> responseData = await dbContext.Recipes
                .Where(r => r.UserId.ToLower() == requestData.UserId.ToLower())
                .Select(r => new GetAllUserRecipesResponseData
                {
                    Id = r.Id,
                    Title = r.Title,
                    ImageUrl = r.ImageUrl,
                    CreatedOn = r.CreatedOn,
                    CategoryName = r.Category.Name
                })
                .OrderBy(r => r.Id)
                .ToArrayAsync();

            return responseData;
        }
    }
}
