using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreJwt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// 取得Claims
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("claims")]
    public IActionResult Get()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            Data = claims
        });
    }

    /// <summary>
    /// 取得使用者基本資訊
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("userInfo")]
    public IActionResult GetUserInfo()
    {
        var userNameCliam = User.Claims.FirstOrDefault(item => item.Type == ClaimTypes.Name);
        return Ok(new
        {
            UserName = userNameCliam.Value
        });
    }
}