using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Genre;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Services.Data.RecipeIngredient;
using Cookbook.Web.ViewModels.Genre;
using Cookbook.Web.ViewModels.Recipe;
using Cookbook.Web.ViewModels.Ingredient;
using Microsoft.AspNetCore.Mvc;

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
                    .OrderBy(r => r.CategoryName)
                    .ThenByDescending(r => r.CreatedOn)
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
    }
}
