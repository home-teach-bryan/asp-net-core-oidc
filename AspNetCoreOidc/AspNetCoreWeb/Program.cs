using System.Security.Claims;
using AspNetCoreWeb.Authorization;
using AspNetCoreWeb.Models.Enum;
using AspNetCoreWeb.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWeb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
        builder.Services.AddSingleton<IAuthorizationHandler, SelfHostAuthorizationHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("EmployeeOnly", policy =>
            {
                policy.RequireRole("Employee");
                policy.RequireClaim(ClaimTypes.Name, "Employee");
                policy.Requirements.Add(new SelfHostRequirement(LoginType.SelfHost));
            });
        });
        
        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}