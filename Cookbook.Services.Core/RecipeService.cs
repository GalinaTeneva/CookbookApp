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
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                Dictionary<int, string> requestedIngredientQuantities = await ResolveIngredientQuantitiesAsync(requestData.RecipeIngredients);

                Recipe newRecipe = new Recipe
                {
                    Title = requestData.Title,
                    Description = requestData.Description,
                    Instructions = requestData.Instructions,
                    ImageUrl = requestData.ImageUrl,
                    CreatedOn = requestData.CreatedOn,
                    CategoryId = requestData.CategoryId,
                    UserId = requestData.UserId,
                    RecipeIngredients = requestedIngredientQuantities
                    .Select(i => new RecipeIngredient
                    {
                        IngredientId = i.Key,
                        Quantity = i.Value,
                    })
                    .ToArray()
                };

                await dbContext.Recipes.AddAsync(newRecipe);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured while adding a new recipe.");
                return false;
            }
        }

        public async Task<bool> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator)
                return false;

            Recipe? editingRecipe = await dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.Id == requestData.Id);
            if (editingRecipe == null)
                return false;

            try
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                Dictionary<int, string> requestedIngredientQuantities = await ResolveIngredientQuantitiesAsync(requestData.RecipeIngredients);

                foreach (RecipeIngredient existingIngredient in editingRecipe.RecipeIngredients.ToList())
                {
                    if (requestedIngredientQuantities.TryGetValue(existingIngredient.IngredientId, out string? quantity))
                    {
                        existingIngredient.Quantity = quantity;
                        requestedIngredientQuantities.Remove(existingIngredient.IngredientId);
                    }
                    else
                    {
                        editingRecipe.RecipeIngredients.Remove(existingIngredient);
                        dbContext.RecipesIngredients.Remove(existingIngredient);
                    }
                }

                foreach (KeyValuePair<int, string> ingredient in requestedIngredientQuantities)
                {
                    editingRecipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = editingRecipe.Id,
                        IngredientId = ingredient.Key,
                        Quantity = ingredient.Value
                    });
                }

                editingRecipe.Title = requestData.Title;
                editingRecipe.Description = requestData.Description;
                editingRecipe.Instructions = requestData.Instructions;
                editingRecipe.ImageUrl = requestData.ImageUrl;
                editingRecipe.CategoryId = requestData.CategoryId;

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured while editing a recipe.");
                return false;
            }
        }

        public async Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData)
        {
            if (string.IsNullOrWhiteSpace(requestData.UserId))
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

        public async Task<GetRecipeDetailsResponseData?> GetRecipeDetailsAsync(int recipeId)
        {
            return await dbContext.Recipes
                .Where(r => r.Id == recipeId)
                .Select(r => new GetRecipeDetailsResponseData()
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    InstructionSteps = r.Instructions.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                    ImageUrl = r.ImageUrl,
                    CreatedOn = r.CreatedOn,
                    CategoryId = r.Category.Id,
                    CategoryName = r.Category.Name,
                    UserId = r.UserId,
                    CreatedBy = r.CreatedBy.UserName ?? "Unknown cook",
                    RecipeIngredients = r.RecipeIngredients
                    .OrderBy(ri => ri.Ingredient.Name)
                    .Select(ri => new RecipeIngredientsResponseData()
                    {
                        IngredientName = ri.Ingredient.Name,
                        Quantity = ri.Quantity
                    })
                    .ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<GetRecipeForEditResponseData?> GetRecipeForEditAsync(RecipeAccessRequestData accessRequestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator)
                return null;

            GetRecipeForEditResponseData? editingRecipe = await dbContext.Recipes
                .Where(r => r.Id == accessRequestData.RecipeId)
                .Select(r => new GetRecipeForEditResponseData
                {
                    Title = r.Title,
                    Description = r.Description,
                    Instructions = r.Instructions,
                    ImageUrl = r.ImageUrl,
                    CategoryId = r.CategoryId,
                    RecipeIngredients = r.RecipeIngredients
                    .Select(ri => new RecipeIngredientsResponseData()
                    {
                        IngredientName = ri.Ingredient.Name,
                        Quantity = ri.Quantity
                    })
                    .OrderBy(ri => ri.IngredientName)
                    .ToList()
                })
                .SingleOrDefaultAsync();

            return editingRecipe;
        }

        public async Task<bool> DeleteRecipeAsync(RecipeAccessRequestData accessRequestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator)
                return false;

            try
            {
                Recipe? deletingRecipe = await dbContext.Recipes.FirstOrDefaultAsync(r => r.Id == accessRequestData.RecipeId);
                if (deletingRecipe == null)
                    return false;

                dbContext.Recipes.Remove(deletingRecipe);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                logger.LogError(e, "An error occurred while deleting recipe {RecipeId}.", accessRequestData.RecipeId);

                return false;
            }
        }

        public async Task<DeleteRecipeResponseData?> GetRecipeDeleteDetailsAsync(RecipeAccessRequestData accessRequestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator)
                return null;

            DeleteRecipeResponseData? deleteRecipe = await dbContext.Recipes
                .Where(r => r.Id == accessRequestData.RecipeId)
                .Select(r => new DeleteRecipeResponseData
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .SingleOrDefaultAsync();

            return deleteRecipe;
        }

        public Task<bool> IsUserCreatorAsync(RecipeAccessRequestData requestData)
        {
            return dbContext.Recipes.AnyAsync(r => r.Id == requestData.RecipeId && r.UserId == requestData.UserId);
        }

        private async Task<Dictionary<int, string>> ResolveIngredientQuantitiesAsync(IEnumerable<RecipeIngredientRequestData> requestedIngredients)
        {
            Dictionary<int, string> ingredientQuantities = new Dictionary<int, string>();

            foreach (var item in requestedIngredients)
            {
                IngredientResponseData ingredient = await ingredientService.FindOrCreateAsync(
                    new IngredientRequestData
                    {
                        Name = item.IngredientName
                    });

                if (!ingredientQuantities.TryAdd(ingredient.Id, item.Quantity))
                {
                    throw new ArgumentException($"Ingredient '{ingredient.Name}' appears more than once.", nameof(requestedIngredients));
                }
            }

            return ingredientQuantities;
        }

    }  
}
