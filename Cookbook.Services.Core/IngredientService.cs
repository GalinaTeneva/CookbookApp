using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Ingredient.Request;
using Cookbook.Services.Data.Ingredient.Response;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Services.Core
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        public async Task<IngredientResponseData> FindOrCreateAsync(IngredientRequestData requestData)
        {
            if(string.IsNullOrWhiteSpace(requestData.Name))
            {
                throw new ArgumentException("Ingredient name cannot be empty", nameof(requestData));
            }

            string normalizedName = requestData.Name.Trim().ToLowerInvariant();

            Ingredient? ingredient = await ingredientRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(i => i.Name.ToLower() == normalizedName);

            if(ingredient ==  null)
            {
                ingredient = new Ingredient { Name = normalizedName };

                await ingredientRepository.AddAsync(ingredient);
                await ingredientRepository.SaveChangesAsync();
            }

            return new IngredientResponseData
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }
    }
}
