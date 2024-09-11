using AspNetCoreJwt.Jwt;
using AspNetCoreJwt.Models;
using AspNetCoreJwt.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreJwt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public TokenController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    /// <summary>
    /// 產生Token
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("")]
    public IActionResult GenerateToken([FromBody]GenerateTokenRequest request)
    {
        var (isValid, user) = _userService.IsValid(request.Name, request.Password);
        if (!isValid)
        {
            return BadRequest();
        }
        var token = _jwtTokenGenerator.GenerateJwtToken(user.Id, user.Name, user.Roles);
        return Ok(new
        {
            Data = token
        });
    }
}