using Cookbook.Data;
using Cookbook.Data.Models;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Ingredient.Request;
using Cookbook.Services.Data.Ingredient.Response;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Services.Data.RecipeIngredient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cookbook.Services.Core
{
    public class RecipeService : IRecipeService
    {
        private readonly CookbookDbContext dbContext;
        private readonly ILogger<RecipeService> logger;

        private IIngredientService ingredientService;

        public RecipeService(CookbookDbContext dbContext, ILogger<RecipeService> logger, IIngredientService ingredientService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.ingredientService = ingredientService;
        }

        public async Task<bool> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData)
        {
            try
            {
                List<RecipeIngredientRequestData> requestedIngredients = requestData.RecipeIngredients.ToList();

                List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
                HashSet<int> ingredientIds = new HashSet<int>();

                foreach (var item in requestedIngredients)
                {
                    IngredientResponseData ingredient =
                        await ingredientService.FindOrCreateAsync(
                            new IngredientRequestData
                            {
                                Name = item.IngredientName
                            });

                    if (!ingredientIds.Add(ingredient.Id))
                    {
                        throw new ArgumentException(
                            $"Ingredient '{ingredient.Name}' appears more than once.", nameof(requestData));
                    }

                    recipeIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = ingredient.Id,
                        Quantity = item.Quantity
                    });
                }

                Recipe newRecipe = new Recipe
                {
                    Title = requestData.Title,
                    Description = requestData.Description,
                    Instructions = requestData.Instructions,
                    ImageUrl = requestData.ImageUrl,
                    CreatedOn = requestData.CreatedOn,
                    CategoryId = requestData.CategoryId,
                    UserId = requestData.UserId,
                    RecipeIngredients = recipeIngredients
                };

                await dbContext.Recipes.AddAsync(newRecipe);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "An error occured while adding a new recipe.");
                return false;
            }
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
