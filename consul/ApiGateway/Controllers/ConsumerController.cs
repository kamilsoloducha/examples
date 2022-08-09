using Blueprints;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[Route("[controller]")]
public class ConsumerController : ControllerBase
{
    private readonly string _serviceName = "service-name";
    private readonly IServiceDiscoveryUrlProvider _serviceDiscoveryUrlProvider;
    private readonly HttpClient _httpClient;

    public ConsumerController(IServiceDiscoveryUrlProvider serviceDiscoveryUrlProvider, HttpClient httpClient)
    {
        _serviceDiscoveryUrlProvider = serviceDiscoveryUrlProvider;
        _httpClient = httpClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var uri = await _serviceDiscoveryUrlProvider.GetUrls(_serviceName, "test", cancellationToken);
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        return Ok(responseString);
    }
}