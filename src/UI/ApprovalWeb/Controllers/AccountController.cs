using Microsoft.AspNetCore.Mvc;
using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using System.Threading.Tasks;

namespace ApprovalWeb.Controllers
{
    /// <summary>
    /// Controller responsible for handling account-related actions such as login and logout.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">Service for authentication operations.</param>
        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <param name="returnUrl">URL to redirect to after successful login.</param>
        /// <returns>Login view.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Handles login requests.
        /// </summary>
        /// <param name="model">Login view model containing user credentials.</param>
        /// <returns>Redirects to the specified return URL if successful, otherwise reloads the login view with an error.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (await _authService.SignInAsync(model.EmailId, model.Password))
            {
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Invalid login.");
            return View(model);
        }

        /// <summary>
        /// Handles logout requests.
        /// </summary>
        /// <returns>Redirects to the login page after logout.</returns>
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
