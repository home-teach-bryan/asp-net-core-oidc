using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOidc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("claims")]
        public IActionResult Get()
        {
            var Claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(Claims);
        }
    }
}
