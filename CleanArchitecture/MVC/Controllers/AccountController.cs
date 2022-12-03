using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models.Account;

namespace MVC.Controllers;


public class AccountController : Controller
{
    private readonly IAuthenticationService authService;

    public AccountController(IAuthenticationService authService)
    {
        this.authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            returnUrl ??= Url.Content("~/");
            var isLoggedIn = await authService.Authenticate(loginVM.Email, loginVM.Password);

            if (isLoggedIn)
                return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("", "Login failed");

        return View(loginVM);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (ModelState.IsValid)
        {
            var returnUrl = Url.Content("~/");
            var isCreated = await authService.RegisterAsync(registerVM);   
            if (isCreated)
                return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        await authService.Logout();
        return LocalRedirect(returnUrl);
    }
}
