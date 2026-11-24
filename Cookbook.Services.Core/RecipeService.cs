using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;
using Cookbook.Services.Core.Contracts;
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
        private readonly IRecipeRepository recipeRepository;
        private readonly IIngredientRepository ingredientRepository;
        private readonly ILogger<RecipeService> logger;


        public RecipeService(IRecipeRepository recipeRepository, ILogger<RecipeService> logger, IIngredientRepository ingredientRepository)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.logger = logger;
        }

        public async Task<bool> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData)
        {
            try
            {
                Dictionary<Ingredient, string> requestedIngredientQuantities = await ResolveIngredientQuantitiesAsync(requestData.RecipeIngredients);

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
                        Ingredient = i.Key,
                        Quantity = i.Value,
                    })
                    .ToArray()
                };

                await recipeRepository.AddAsync(newRecipe);
                await recipeRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while adding a new recipe.");
                return false;
            }
        }

        public async Task<bool> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator || requestData.Id != accessRequestData.RecipeId)
                return false;

            try
            {
                Recipe? editingRecipe = await recipeRepository
                    .GetAll()
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .FirstOrDefaultAsync(r => r.Id == requestData.Id);
                if (editingRecipe == null)
                    return false;

                Dictionary<Ingredient, string> requestedIngredientQuantities = await ResolveIngredientQuantitiesAsync(requestData.RecipeIngredients);

                foreach (RecipeIngredient existingIngredient in editingRecipe.RecipeIngredients.ToList())
                {
                    if (requestedIngredientQuantities.TryGetValue(existingIngredient.Ingredient, out string? quantity))
                    {
                        existingIngredient.Quantity = quantity;
                        requestedIngredientQuantities.Remove(existingIngredient.Ingredient);
                    }
                    else
                    {
                        editingRecipe.RecipeIngredients.Remove(existingIngredient);
                    }
                }

                foreach (KeyValuePair<Ingredient, string> ingredient in requestedIngredientQuantities)
                {
                    editingRecipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = editingRecipe.Id,
                        Ingredient = ingredient.Key,
                        Quantity = ingredient.Value
                    });
                }

                editingRecipe.Title = requestData.Title;
                editingRecipe.Description = requestData.Description;
                editingRecipe.Instructions = requestData.Instructions;
                editingRecipe.ImageUrl = requestData.ImageUrl;
                editingRecipe.CategoryId = requestData.CategoryId;

                await recipeRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while editing a recipe.");
                return false;
            }
        }

        public async Task<IEnumerable<GetAllUserRecipesResponseData>> GetAllUserRecipesAsync(GetAllUserRecipesRequestData requestData)
        {
            if (string.IsNullOrWhiteSpace(requestData.UserId))
            {
                throw new ArgumentException("UserId can not be null or empty", nameof(requestData.UserId));
            }

            IEnumerable<GetAllUserRecipesResponseData> responseData = await recipeRepository
                .AllAsNoTracking()
                .Where(r => r.UserId == requestData.UserId)
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

        public async Task<GetRecipeDetailsResponseData?> GetRecipeDetailsAsync(RecipeAccessRequestData accessRequestData)
        {
            bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
            if (!isUserCreator)
                return null;

            return await recipeRepository
                .AllAsNoTracking()
                .Where(r => r.Id == accessRequestData.RecipeId)
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

            GetRecipeForEditResponseData? editingRecipe = await recipeRepository
                .AllAsNoTracking()
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
                Recipe? deletingRecipe = await recipeRepository.FindByIdAsync(accessRequestData.RecipeId);
                if (deletingRecipe == null)
                    return false;

                recipeRepository.Remove(deletingRecipe);
                await recipeRepository.SaveChangesAsync();

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

            DeleteRecipeResponseData? deleteRecipe = await recipeRepository
                .AllAsNoTracking()
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
            return recipeRepository
                .AllAsNoTracking()
                .AnyAsync(r => r.Id == requestData.RecipeId && r.UserId == requestData.UserId);
        }

        private async Task<Dictionary<Ingredient, string>> ResolveIngredientQuantitiesAsync(IEnumerable<RecipeIngredientRequestData> requestedIngredients)
        {
            Dictionary<string, string> requestedQuantities = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in requestedIngredients)
            {
                if (string.IsNullOrWhiteSpace(item.IngredientName))
                    throw new ArgumentException("Ingredient name can not be empty.");

                string name = item.IngredientName.Trim();

                if (!requestedQuantities.TryAdd(name, item.Quantity))
                    throw new ArgumentException($"Ingredient \"{name}\" appears more than once.");
            }

            Dictionary<Ingredient, string> resolvedIngredients = new Dictionary<Ingredient, string>();

            foreach (var item in requestedQuantities)
            {
                string name = item.Key.ToLowerInvariant();
                string quantity = item.Value;

                Ingredient? ingredient = await ingredientRepository.GetAll().FirstOrDefaultAsync(i => i.Name.ToLower() == name);

                if(ingredient == null)
                {
                    ingredient = new Ingredient()
                    {
                        Name = name,
                    };

                    ingredientRepository.Add(ingredient);

                }    
                    resolvedIngredients.Add(ingredient, quantity);
            }

            return resolvedIngredients;
        }
    }  
}
