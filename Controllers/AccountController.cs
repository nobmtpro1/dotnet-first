using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Blog.ViewModels.Account;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers;

public class AccountController : BaseController
{
    protected UserManager<UserModel> _userManager;
    protected SignInManager<UserModel> _signInManager;

    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<UserModel> userManage, SignInManager<UserModel> signInManager, ILogger<AccountController> logger)
    {
        _userManager = userManage;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewData["Error"] = "Email or password incorrect";
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

}
