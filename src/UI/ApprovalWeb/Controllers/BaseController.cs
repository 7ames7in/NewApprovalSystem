using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BaseController : Controller
{

    // public override void OnActionExecuting(ActionExecutingContext context)
    // {
    //     ApprovalWeb.Interfaces.IAuthenticationService authService = context.HttpContext.RequestServices.GetService(typeof(ApprovalWeb.Interfaces.IAuthenticationService)) as ApprovalWeb.Interfaces.IAuthenticationService ?? throw new InvalidOperationException("Authentication service is not available.");
    //     if (authService == null)
    //     {
    //         throw new InvalidOperationException("Authentication service is not available.");
    //     }

    //     if (User.Identity?.IsAuthenticated == true)
    //     {
    //         var email = User.Identity.Name ?? string.Empty;
    //         if (authService.SignInAsync(email, string.Empty).Result)
    //         {
    //             // if (string.IsNullOrEmpty(userId))
    //             // context.Result = RedirectToAction("Login", "Account");
    //             // Assuming SignInAsync updates the user context or session
    //         }
    //         else
    //         {
    //             // Handle the case where sign-in fails, e.g., redirect to login page
    //             context.Result = RedirectToAction("Login", "Account");
    //             return;
    //         }
    //         //ViewBag.CurrentUser = user;
    //     }
    //     base.OnActionExecuting(context);
    // }
}