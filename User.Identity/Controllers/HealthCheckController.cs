using Microsoft.AspNetCore.Mvc;

namespace User.Identity.Controllers
{
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("")]
        [HttpPost("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}