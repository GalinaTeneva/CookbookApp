using Cookbook.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cookbook.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            switch(statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    return View("BadRequest");
                case StatusCodes.Status404NotFound:
                    return View("NotFound");
                case StatusCodes.Status403Forbidden:
                    return View("Forbidden");
                case null:
                case StatusCodes.Status500InternalServerError:
                    return View("ServerError");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
