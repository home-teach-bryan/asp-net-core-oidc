using System.Security.Claims;
using AspNetCoreWeb.Models;
using AspNetCoreWeb.Models.Enum;
using AspNetCoreWeb.Service;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWeb.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AccountController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var (isValid, user) = _userService.IsValid(model.Username, model.Password);
        if (!isValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }        
        await ProcessSignInAsync(user.Name, user.Roles, LoginType.SelfHost);
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Account/Login");
    }

    public async Task<IActionResult> GoogleVerify()
    {
        var credential = HttpContext.Request.Form["credential"];
        var formToken = HttpContext.Request.Form["g_csrf_token"];
        var cookiesToken = HttpContext.Request.Cookies["g_csrf_token"];
        if (string.IsNullOrEmpty(credential) || string.IsNullOrEmpty(formToken) || string.IsNullOrEmpty(cookiesToken) ||
            formToken != cookiesToken)
        {
            return RedirectToAction("Login", "Account");
        }
        var googleClientId = _configuration.GetValue<string>("Authentication:Google:ClientId");
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { googleClientId }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
        if (payload.Issuer != "accounts.google.com" && payload.Issuer != "https://accounts.google.com")
        {
            return RedirectToAction("Login", "Account");
        }
        await ProcessSignInAsync(payload.Name, new string[] { "Admin" }, LoginType.Google);
        return RedirectToAction("Index", "Home");
    }

    private async Task ProcessSignInAsync(string userName, string[] userRoles, LoginType loginType)
    {
        var rolesClaim = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(nameof(LoginType), loginType.ToString()),
        };
        claims.AddRange(rolesClaim);
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}