using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Genre;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Web.ViewModels.Genre;
using Cookbook.Web.ViewModels.Recipe;
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
            {
                return Unauthorized();
            }

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

        //TODO: Add logic for saving the Ingredients in the DB
        [HttpPost]
        public async Task<IActionResult> Add(AddRecipeInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel.Categories = await GetAllCategoriesViewModelAsync();
                return View(inputModel);
            }

            bool isCategoryValid = await categoryService.CategoryExistsAsync(inputModel.CategoryId);
            if(!isCategoryValid)
            {
                return BadRequest("There was an error with your request!");
            }

            AddRecipeToUserRecipeListRequestData addRecipeToUserRecipeList = new AddRecipeToUserRecipeListRequestData()
            {
                Title = inputModel.Title,
                Description = inputModel.Description,
                Instructions = inputModel.Instructions,
                ImageUrl = inputModel.ImageUrl,
                CreatedOn = DateTime.Now,
                CategoryId = inputModel.CategoryId,
                UserId = GetUserId()!
            };

            bool isRecipeCreated = await recipeService.AddRecipeToUserRecipeListAsync(addRecipeToUserRecipeList);
            if(!isRecipeCreated)
            {
                ModelState.AddModelError(string.Empty, "There was an unexpected error with your request. Please try again!");
                inputModel.Categories = await GetAllCategoriesViewModelAsync();

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
    }
}
