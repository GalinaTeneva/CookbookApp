using Cookbook.Data.Models;
using Cookbook.Data.Repository.Contracts;
using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Ingredient.Response;
using Cookbook.Services.Data.Recipe.Enums;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Services.Data.RecipeIngredient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

using static Cookbook.Common.ErrorMessages.Ingredient;

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

        public async Task<RecipeServiceStatusResponseModel> AddRecipeToUserRecipeListAsync(AddRecipeToUserRecipeListRequestData requestData)
        {
            RecipeServiceStatusResponseModel response = new RecipeServiceStatusResponseModel();

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

                response.Status = RecipeServiceResponseStatus.Success;
            }
            catch (ValidationException ve)
            {
                response.Status = RecipeServiceResponseStatus.InvalidRequest;
                response.ErrorMessages = new List<string> { ve.Message };
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while adding a new recipe.");
                response.Status = RecipeServiceResponseStatus.GeneralError;
                response.ErrorMessages = new List<string>() { e.Message };
            }

            return response;
        }

        public async Task<RecipeServiceStatusResponseModel> EditRecipeAsync(RecipeAccessRequestData accessRequestData, RecipeForEditRequestData requestData)
        {
            RecipeServiceStatusResponseModel response = new RecipeServiceStatusResponseModel()
            {
                Status = RecipeServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string>()
            };

            try
            {
                if (requestData.Id != accessRequestData.RecipeId)
                {
                    response.Status = RecipeServiceResponseStatus.InvalidRequest;
                    return response;
                }

                Recipe? editingRecipe = await recipeRepository
                    .GetAll()
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .FirstOrDefaultAsync(r => r.Id == requestData.Id && r.UserId == accessRequestData.UserId);

                if (editingRecipe == null)
                {
                    response.Status = RecipeServiceResponseStatus.NotFound;

                    return response;
                }

                Dictionary<Ingredient, string> requestedIngredientQuantities = await ResolveIngredientQuantitiesAsync(requestData.RecipeIngredients);

                SynchronizeRecipeIngredients(editingRecipe, requestedIngredientQuantities);

                editingRecipe.Title = requestData.Title;
                editingRecipe.Description = requestData.Description;
                editingRecipe.Instructions = requestData.Instructions;
                editingRecipe.ImageUrl = requestData.ImageUrl;
                editingRecipe.CategoryId = requestData.CategoryId;

                await recipeRepository.SaveChangesAsync();

                response.Status = RecipeServiceResponseStatus.Success;
            }
            catch (ValidationException ve)
            {
                response.Status = RecipeServiceResponseStatus.InvalidRequest;
                response.ErrorMessages = new List<string> { ve.Message };
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while editing a recipe.");
                response.Status = RecipeServiceResponseStatus.GeneralError;
                response.ErrorMessages = new List<string>() { e.Message };
            }

            return response;
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
            return await recipeRepository
                .AllAsNoTracking()
                .Where(r => r.Id == accessRequestData.RecipeId && r.UserId == accessRequestData.UserId)
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
            GetRecipeForEditResponseData? editingRecipe = await recipeRepository
                .AllAsNoTracking()
                .Where(r => r.Id == accessRequestData.RecipeId && r.UserId == accessRequestData.UserId)
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

        public async Task<RecipeServiceStatusResponseModel> DeleteRecipeAsync(RecipeAccessRequestData accessRequestData)
        {
            RecipeServiceStatusResponseModel responseData = new RecipeServiceStatusResponseModel()
            {
                Status = RecipeServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string>()
            };

            try
            {
                Recipe? deletingRecipe = await recipeRepository
                    .GetAll()
                    .SingleOrDefaultAsync(r => r.Id == accessRequestData.RecipeId && r.UserId == accessRequestData.UserId);
                if (deletingRecipe == null)
                {
                    responseData.Status = RecipeServiceResponseStatus.NotFound;
                    return responseData;
                }

                recipeRepository.Remove(deletingRecipe);
                await recipeRepository.SaveChangesAsync();

                responseData.Status = RecipeServiceResponseStatus.Success;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while deleting recipe {RecipeId}.", accessRequestData.RecipeId);
                responseData.Status = RecipeServiceResponseStatus.GeneralError;
                responseData.ErrorMessages = new List<string> { (e.Message) };
            }

            return responseData;
        }

        public async Task<DeleteRecipeResponseData?> GetRecipeDeleteDetailsAsync(RecipeAccessRequestData accessRequestData)
        {
            DeleteRecipeResponseData? deleteRecipe = await recipeRepository
                .AllAsNoTracking()
                .Where(r => r.Id == accessRequestData.RecipeId && r.UserId == accessRequestData.UserId)
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
            Dictionary<string, string> requestedQuantities = new(StringComparer.OrdinalIgnoreCase);

            foreach (RecipeIngredientRequestData item in requestedIngredients)
            {
                if (string.IsNullOrWhiteSpace(item.IngredientName))
                {
                    throw new ValidationException(NameRequiredMessage);
                }

                string trimmedName = item.IngredientName.Trim();
                string normalizedName = trimmedName.ToLowerInvariant();

                if (!requestedQuantities.TryAdd(normalizedName, item.Quantity))
                {
                    throw new ValidationException(
                        $"Ingredient \"{trimmedName}\" appears more than once.");
                }
            }

            if (requestedQuantities.Count == 0)
            {
                return new Dictionary<Ingredient, string>();
            }

            List<Ingredient> existingIngredients =
                await ingredientRepository.FindByNormalizedNamesAsync(
                    requestedQuantities.Keys.ToArray());

            Dictionary<string, Ingredient> ingredientsByName =
                new(StringComparer.OrdinalIgnoreCase);

            foreach (Ingredient ingredient in existingIngredients)
            {
                ingredientsByName.TryAdd(ingredient.Name, ingredient);
            }

            Dictionary<Ingredient, string> resolvedIngredients = new();

            foreach (KeyValuePair<string, string> entry in requestedQuantities)
            {
                if (!ingredientsByName.TryGetValue(
                    entry.Key,
                    out Ingredient? ingredient))
                {
                    ingredient = new Ingredient
                    {
                        Name = entry.Key
                    };

                    ingredientRepository.Add(ingredient);
                    ingredientsByName.Add(entry.Key, ingredient);
                }

                resolvedIngredients.Add(ingredient, entry.Value);
            }

            return resolvedIngredients;
        }

        private static void SynchronizeRecipeIngredients(Recipe recipe, IReadOnlyDictionary<Ingredient, string> requestedQuantities)
        {
            Dictionary<Ingredient, string> remainingIngredients =
                requestedQuantities.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value);

            foreach (RecipeIngredient existing in recipe.RecipeIngredients.ToList())
            {
                if (remainingIngredients.TryGetValue(
                    existing.Ingredient,
                    out string? quantity))
                {
                    existing.Quantity = quantity;
                    remainingIngredients.Remove(existing.Ingredient);
                }
                else
                {
                    recipe.RecipeIngredients.Remove(existing);
                }
            }

            foreach (KeyValuePair<Ingredient, string> entry in remainingIngredients)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    Ingredient = entry.Key,
                    Quantity = entry.Value
                });
            }
        }

    }
}
