using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Health() => Ok();
}