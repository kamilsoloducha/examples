using System.Threading;
using Blueprints;
using Blueprints.Events;
using Blueprints.Rabbit;
using Blueprints.Serilog;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Service1.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.ConfigureMassTransit(configuration);
builder.Services.ConfigureSerilog(configuration);
builder.Services.AddSingleton<IServiceIdentificator, Service1Identificator>();
builder.Services.AddHostedService<BusService>();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseRouting();

app.MapGet("service2/{value}", async (
    [FromRoute] string value,
    IPublishEndpoint publishEndpoint,
    CancellationToken cancellationToken) =>
{
    await publishEndpoint.Publish(new Service2Event { Value = value }, cancellationToken);
    return Results.Ok();
});

app.MapGet("service2and3/{value}", async (
    [FromRoute] string value,
    IPublishEndpoint publishEndpoint,
    CancellationToken cancellationToken) =>
{
    await publishEndpoint.Publish(new Service2and3Event { Value = value }, cancellationToken);
    return Results.Ok();
});

app.MapGet("service3th2/{value}", async (
    [FromRoute] string value,
    IPublishEndpoint publishEndpoint,
    CancellationToken cancellationToken) =>
{
    await publishEndpoint.Publish(new Service3th2Event { Value = value }, cancellationToken);
    return Results.Ok();
});

await app.RunAsync();