using Cookbook.Services.Core.Contracts;
using Cookbook.Services.Data.Recipe.Request;
using Cookbook.Services.Data.Recipe.Response;
using Cookbook.Web.ViewModels.Recipe;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> MyRecipes()
        {
            string? userId = GetUserId();
            if(userId == null)
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
            catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
        }
    }
}
