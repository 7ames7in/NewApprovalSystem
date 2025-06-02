using Microsoft.AspNetCore.Mvc;
using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using System.Threading.Tasks;

namespace ApprovalWeb.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;

    public AccountController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/") => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (await _authService.SignInAsync(model.EmailId, model.Password))
            return LocalRedirect(model.ReturnUrl ?? "/");

        ModelState.AddModelError("", "Invalid login.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("Login");
    }
}
