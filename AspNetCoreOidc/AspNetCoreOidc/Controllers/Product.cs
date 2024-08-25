using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreOidc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {

            var Claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(Claims);
        }
    }
}
