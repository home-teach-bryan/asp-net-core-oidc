using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        var user = HttpContext.User.Claims;
        var userNameCliam = user.FirstOrDefault(item => item.Type == ClaimTypes.Name);
        if (userNameCliam == null)
        {
            return RedirectToAction("Login", "Account");
        }
        ViewData["UserName"] = userNameCliam.Value;
        return View();

    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Claims()
    {
        var claims = HttpContext.User.Claims;
        ViewData["Claims"] = claims;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}