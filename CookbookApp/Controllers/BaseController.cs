using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            return this.User?.Identity?.IsAuthenticated ?? false;
        }

        protected string? GetUserId()
        {
            if(!this.IsUserAuthenticated())
            {
                return null;
            }
            return this.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
