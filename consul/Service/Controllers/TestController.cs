using Blueprints;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers;

[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IServiceIdProvider<Guid> _guidServiceIdProvider;

    public TestController(ILogger<TestController> logger,
        IServiceIdProvider<Guid> guidServiceIdProvider)
    {
        _logger = logger;
        _guidServiceIdProvider = guidServiceIdProvider;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Start");
        //await Task.Delay(5000);
        _logger.LogInformation("Finish");
        return Ok(new
        {
            ServiceId = _guidServiceIdProvider.GetId().ToString()
        });
    }
}