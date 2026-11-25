using Azure;
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
            catch(ValidationException ve)
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
                if(requestData.Id != accessRequestData.RecipeId)
                {
                    response.Status = RecipeServiceResponseStatus.InvalidRequest;
                    return response;
                }

                bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
                if (!isUserCreator)
                {
                    response.Status = RecipeServiceResponseStatus.NotFound;
                    return response;
                }

                Recipe? editingRecipe = await recipeRepository
                    .GetAll()
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .FirstOrDefaultAsync(r => r.Id == requestData.Id);
                
                if(editingRecipe != null)
                {
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

                    response.Status = RecipeServiceResponseStatus.Success;
                }
                else
                {
                    response.Status = RecipeServiceResponseStatus.NotFound;
                }
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

        public async Task<RecipeServiceStatusResponseModel> DeleteRecipeAsync(RecipeAccessRequestData accessRequestData)
        {
            RecipeServiceStatusResponseModel responseData = new RecipeServiceStatusResponseModel()
            {
                Status = RecipeServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string>()
            };

            try
            {
                bool isUserCreator = await this.IsUserCreatorAsync(accessRequestData);
                if (!isUserCreator)
                {
                    responseData.Status = RecipeServiceResponseStatus.NotFound;
                    return responseData;
                }

                Recipe? deletingRecipe = await recipeRepository.FindByIdAsync(accessRequestData.RecipeId);
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
                    throw new ValidationException("Ingredient name can not be empty.");

                string name = item.IngredientName.Trim();

                if (!requestedQuantities.TryAdd(name, item.Quantity))
                    throw new ValidationException($"Ingredient \"{name}\" appears more than once.");
            }

            Dictionary<Ingredient, string> resolvedIngredients = new Dictionary<Ingredient, string>();

            foreach (var item in requestedQuantities)
            {
                string name = item.Key.ToLowerInvariant();
                string quantity = item.Value;

                Ingredient? ingredient = await ingredientRepository.GetAll().FirstOrDefaultAsync(i => i.Name.ToLower() == name);

                if (ingredient == null)
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
