using Api.Redis;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class TestController
{
    private static readonly Guid TestValue = Guid.NewGuid();
    
    [HttpGet]
    public string Test() => TestValue.ToString();
}

[Route("[controller]")]
public class DataController(IRedisProvider redisProvider, ILogger<DataController> logger)
{
    private const string Key = "key";
    
    [HttpGet("get")]
    public Task<string> Get()
    {
        logger.LogInformation("Getting value from {Key}", Key);
        return redisProvider.GetValue(Key);
    }

    [HttpGet("set/{value}")]
    public Task Set([FromRoute] string value)
    {
        logger.LogInformation("Setting {Value} on {Key}", value, Key);
        return redisProvider.SetValue(Key, value);
    }
}