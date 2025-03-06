using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers;

[ApiController]
[Route("health")]
public class HealthController : Controller
{
    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        return Ok("Service is healthy");
    }
}