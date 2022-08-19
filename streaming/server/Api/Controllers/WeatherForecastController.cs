using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IAsyncEnumerable<WeatherForecast> Get(CancellationToken cancellationToken)
    {
        var streamRequest = new GetWeatherQuery();
        return _mediator.CreateStream(streamRequest,cancellationToken);
    }
    
    [HttpGet("test")]
    public async IAsyncEnumerable<int> Test([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var i = 0;
        while(!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
            yield return i++;
        }
    }
    
    // [HttpGet("test")]
    // public WeatherForecast Test()
    // {
    //     return new WeatherForecast()
    //     {
    //         Summary = "test"
    //     };
    // }
}


public class GetWeatherQuery : IStreamRequest<WeatherForecast>
{
    
}

internal class GetWeatherQueryHandler : IStreamRequestHandler<GetWeatherQuery, WeatherForecast>
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public async IAsyncEnumerable<WeatherForecast> Handle(GetWeatherQuery request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
            yield return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }
    }
}