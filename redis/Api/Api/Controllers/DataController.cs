using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Api.Controllers;

[Route("[controller]")]
public class DataController
{

    public DataController()
    {
        
    }

    [HttpGet("test")]
    public void Get()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        var database = redis.GetDatabase();
        var result = database.StringSet("key", "value");
    }
    
}
