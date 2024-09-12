using AspNetCoreWeb.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWeb.Authorization;

public class SelfHostAuthorizationHandler : AuthorizationHandler<SelfHostRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfHostRequirement requirement)
    {
        var loginTypeClaim = context.User.Claims.FirstOrDefault(item => item.Type == nameof(LoginType));
        if (loginTypeClaim == null)
        {
            return Task.CompletedTask;
        }
        var loginType = loginTypeClaim.Value;
        if (loginType != requirement.LoginType.ToString())
        {
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}