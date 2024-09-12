using AspNetCoreWeb.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWeb.Authorization;

public class SelfHostRequirement : IAuthorizationRequirement
{
    public readonly LoginType LoginType;

    public SelfHostRequirement(LoginType loginType)
    {
        LoginType = loginType;
    }
}
