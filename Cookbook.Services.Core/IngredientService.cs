using Cookbook.Data;
using Cookbook.Data.Models;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Ingredient.Request;
using Cookbook.Services.Data.Ingredient.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cookbook.Services.Core
{
    public class IngredientService : IIngredientService
    {
        private readonly CookbookDbContext dbContext;

        public IngredientService(CookbookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IngredientResponseData> FindOrCreateAsync(IngredientRequestData requestData)
        {
            if(string.IsNullOrWhiteSpace(requestData.Name))
            {
                throw new ArgumentException("Ingredient name cannot be empty", nameof(requestData));
            }

            string normalizedName = requestData.Name.Trim().ToLowerInvariant();

            Ingredient? ingredient = await dbContext.Ingredients.FirstOrDefaultAsync(i => i.Name.ToLower() == normalizedName);

            if(ingredient ==  null)
            {
                ingredient = new Ingredient { Name = requestData.Name };

                await dbContext.Ingredients.AddAsync(ingredient);
                await dbContext.SaveChangesAsync();
            }

            return new IngredientResponseData
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }
    }
}
