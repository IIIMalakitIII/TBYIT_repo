using System.Linq;
using Ave.API.Utils;
using Ave.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : Controller
    {
        protected UserData CurrentUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CurrentUser = AuthHelper.GetUserData(HttpContext.User.Claims.ToList());
        }
    }
}
