using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    public class RecipesController : Controller
    {
        public IActionResult MyRecipes()
        {
            return View();
        }
    }
}
