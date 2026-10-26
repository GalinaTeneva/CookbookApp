using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Genre;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Services.Data.RecipeIngredient;
using Cookbook.Web.ViewModels.Genre;
using Cookbook.Web.ViewModels.Recipe;
using Cookbook.Web.ViewModels.Ingredient;
using Microsoft.AspNetCore.Mvc;
using Cookbook.Services.Data.Ingredient.Response;
using Cookbook.Data.Models;

namespace Cookbook.Controllers
{
    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public RecipesController(IRecipeService recipeService, ICategoryService categoryService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> MyRecipes()
        {
            string? userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            GetAllUserRecipesRequestData requestData = new GetAllUserRecipesRequestData()
            {
                UserId = userId
            };

            try
            {
                IEnumerable<GetAllUserRecipesResponseData> userRecipesResponseData = await recipeService.GetAllUserRecipesAsync(requestData);

                IEnumerable<MyRecipesViewModel> userRecipesVieModel = userRecipesResponseData
                    .OrderByDescending(r => r.CreatedOn)
                    .ThenBy(r => r.CategoryName)
                    .Select(r => new MyRecipesViewModel()
                    {
                        Id = r.Id,
                        Title = r.Title,
                        CreatedOn = r.CreatedOn.ToString(),
                        ImageUrl = r.ImageUrl,
                        CategoryName = r.CategoryName
                    })
                    .ToArray();

                return View(userRecipesVieModel);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AddRecipeInputModel addRecipeInputModel = new AddRecipeInputModel()
            {
                Categories = await GetAllCategoriesViewModelAsync()
            };

            return View(addRecipeInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRecipeInputModel inputModel)
        {
            string? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                inputModel.Categories = await GetAllCategoriesViewModelAsync();
                return View(inputModel);
            }

            bool isCategoryValid = await categoryService.CategoryExistsAsync(inputModel.CategoryId);
            if (!isCategoryValid)
                return BadRequest("There was an error with your request!");

            AddRecipeToUserRecipeListRequestData addRecipeToUserRecipeList = new AddRecipeToUserRecipeListRequestData()
            {
                Title = inputModel.Title,
                Description = inputModel.Description,
                Instructions = inputModel.Instructions,
                RecipeIngredients = MapIngredientsTextToDTO(inputModel.IngredientsText),
                ImageUrl = inputModel.ImageUrl,
                CreatedOn = DateTime.Now,
                CategoryId = inputModel.CategoryId,
                UserId = userId
            };

            bool isRecipeCreated = await recipeService.AddRecipeToUserRecipeListAsync(addRecipeToUserRecipeList);
            if (!isRecipeCreated)
            {
                ModelState.AddModelError(string.Empty, "There was an unexpected error with your request. Please try again!");
                inputModel.Categories = await GetAllCategoriesViewModelAsync();

                return View(inputModel);
            }

            return RedirectToAction(nameof(MyRecipes));
        }

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("There was an error with your request!");

            string? userId = GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            bool isUserCreator = await recipeService.IsUserCreatorAsync(
                new RecipeAccessRequestData
                {
                    RecipeId = id,
                    UserId = userId
                });

            if (!isUserCreator)
                return NotFound();

            GetRecipeDetailsResponseData? responseData = await recipeService.GetRecipeDetailsAsync(id);
            if (responseData == null)
                return NotFound();

            DisplayRecipeViewModel displayRecipeViewModel = new DisplayRecipeViewModel
            {
                Id = responseData.Id,
                Title = responseData.Title,
                Description = responseData.Description,
                InstructionSteps = responseData.InstructionSteps,
                ImageUrl = responseData.ImageUrl,
                CreatedOn = responseData.CreatedOn.ToString(),
                CategoryName = responseData.CategoryName,
                CreatedBy = responseData.CreatedBy,
                RecipeIngredients = responseData.RecipeIngredients
                .Select(i => new RecipeIngredientsViewModel()
                {
                    IngredientName = i.IngredientName,
                    Quantity = i.Quantity
                })
                .ToArray()
            };

            return View(displayRecipeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("There was an error with your request!");

            string? userId = GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            GetRecipeForEditResponseData? responseData = await recipeService.GetRecipeForEditAsync(
                new RecipeAccessRequestData
                {
                    RecipeId = id,
                    UserId = userId
                });

            if (responseData == null)
                return NotFound();

            EditRecipeInputModel inputModel = new EditRecipeInputModel
            {
                Id = id,
                Title = responseData.Title,
                Description = responseData.Description,
                Instructions = responseData.Instructions,
                ImageUrl = responseData.ImageUrl,
                CategoryId = responseData.CategoryId,
                Categories = await GetAllCategoriesViewModelAsync(),
                IngredientsText = MapIngredientsDTOToText(responseData.RecipeIngredients)
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, EditRecipeInputModel inputModel)
        {
            string? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            RecipeAccessRequestData accessRequestData = new RecipeAccessRequestData
            {
                RecipeId = id,
                UserId = userId
            };

            bool isUserCreator = await recipeService.IsUserCreatorAsync(accessRequestData);

            if (!isUserCreator)
                return NotFound();


            if (!ModelState.IsValid)
            {
                inputModel.Categories = await GetAllCategoriesViewModelAsync();
                return View(inputModel);
            }

            bool isCategoryValid = await categoryService.CategoryExistsAsync(inputModel.CategoryId);
            if (!isCategoryValid)
                return BadRequest("There was an error with your request!");

            RecipeForEditRequestData editingRecipeRequestData = new RecipeForEditRequestData
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                Description = inputModel.Description,
                Instructions = inputModel.Instructions,
                RecipeIngredients = MapIngredientsTextToDTO(inputModel.IngredientsText),
                ImageUrl = inputModel.ImageUrl,
                CategoryId = inputModel.CategoryId
            };

            bool isRecipeEdited = await recipeService.EditRecipeAsync(accessRequestData, editingRecipeRequestData);
            if (!isRecipeEdited)
            {
                ModelState.AddModelError(string.Empty, "There was an unexpected error with your request. Please try again!");
                inputModel.Categories = await GetAllCategoriesViewModelAsync();

                return View(inputModel);
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            RecipeAccessRequestData accessRequestData = new RecipeAccessRequestData
            {
                RecipeId = id,
                UserId = userId
            };

            bool isUserCreator = await recipeService.IsUserCreatorAsync(accessRequestData);

            if (!isUserCreator)
                return NotFound();

            DeleteRecipeResponseData? deleteRecipeResponse = await recipeService.GetRecipeDeleteDetailsAsync(accessRequestData);

            if (deleteRecipeResponse == null)
                return NotFound();

            DeleteRecipeViewModel deleteRecipeViewModel = new DeleteRecipeViewModel
            {
                Id = deleteRecipeResponse.Id,
                Title = deleteRecipeResponse.Title
            };

            return View(deleteRecipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, DeleteRecipeViewModel inputModel)
        {
            string? userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            RecipeAccessRequestData accessRequestData = new RecipeAccessRequestData
            {
                RecipeId = id,
                UserId = userId
            };

            bool isUserCreator = await recipeService.IsUserCreatorAsync(accessRequestData);

            if (!isUserCreator)
                return NotFound();

            bool isDeleted = await recipeService.DeleteRecipeAsync(accessRequestData);
            if(!isDeleted)
            {
                DeleteRecipeResponseData? responseData = await recipeService
                    .GetRecipeDeleteDetailsAsync(accessRequestData);
                if (responseData == null)
                    return NotFound();

                inputModel.Id = responseData.Id;
                inputModel.Title = responseData.Title;

                ModelState.AddModelError(string.Empty, "There was an unexpected error with your request. Please try again!");

                return View(inputModel);
            }

            return RedirectToAction(nameof(MyRecipes));
        }


        private async Task<IEnumerable<AllCategoriesViewModel>> GetAllCategoriesViewModelAsync()
        {
            IEnumerable<GetAllCategoriesResponseData> allCategories = await categoryService.GetAllCategoriesAsync();

            IEnumerable<AllCategoriesViewModel> allCategoriesViewModels = allCategories
                .Select(c => new AllCategoriesViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArray();

            return allCategoriesViewModels;
        }

        private static List<RecipeIngredientRequestData> MapIngredientsTextToDTO(string ingredientsText)
        {
            string[] lines = ingredientsText
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n');

            List<RecipeIngredientRequestData> ingredients = new List<RecipeIngredientRequestData>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                string[] parts = lines[i].Split('|', StringSplitOptions.TrimEntries);

                ingredients.Add(new RecipeIngredientRequestData
                {
                    Quantity = parts[0],
                    IngredientName = parts[1]
                });
            }

            return ingredients;
        }

        private string MapIngredientsDTOToText(IEnumerable<RecipeIngredientsResponseData> recipeIngredients)
        {
            return string.Join(Environment.NewLine,
                recipeIngredients.Select(i => $"{i.Quantity} | {i.IngredientName}"));
        }
    }
}
