using System;
using System.Threading;
using Blueprints.RabbitMassTransit;
using Blueprints.Serilog;
using EventBus;
using EventBus.Abstraction;
using EventBus.ErrorHandling;
using EventBus.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Service1.EventHandlers;
using Service1.EventHandlers.Events;
using Service1.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//builder.Services.ConfigureMassTransit(configuration);
builder.Services.ConfigureSerilog(configuration);
builder.Services.AddSingleton<IServiceIdentificator, Service1Identificator>();
// builder.Services.AddHostedService<BusService>();

builder.Services.AddSingleton<IFatalExceptionChecker>(_ => new DefaultExceptionChecker(
    typeof(InvalidOperationException), typeof(ArgumentException), typeof(ArgumentNullException)));
builder.Services.AddDecoratedEventHandler<Process1Finished, Process1FinishedHandler>();
builder.Services.AddDecoratedEventHandler<Process2Finished, Process2FinishedHandler>();

builder.Services.AddRabbitMq(configuration, async (eventBus, cancellationToken) =>
{
    await eventBus.Subscribe<Process1Finished>(cancellationToken);
    await eventBus.Subscribe<Process2Finished>(cancellationToken);
});

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseRouting();

app.MapGet("process1", async (
    [FromServices] IEventBusPublisher publisher,
    CancellationToken cancellationToken) =>
{
    await publisher.Publish(new Process1Finished(), cancellationToken);
    return Results.Ok();
});

app.MapGet("process2", async (
    [FromServices] IEventBusPublisher publisher,
    CancellationToken cancellationToken) =>
{
    await publisher.Publish(new Process2Finished(), cancellationToken);
    return Results.Ok();
});
//
// app.MapGet("service2/{value}", async (
//     [FromRoute] string value,
//     IPublishEndpoint publishEndpoint,
//     CancellationToken cancellationToken) =>
// {
//     await publishEndpoint.Publish(new Service2Event { Value = value }, cancellationToken);
//     return Results.Ok();
// });
//
// app.MapGet("service2and3/{value}", async (
//     [FromRoute] string value,
//     IPublishEndpoint publishEndpoint,
//     CancellationToken cancellationToken) =>
// {
//     await publishEndpoint.Publish(new Service2and3Event { Value = value }, cancellationToken);
//     return Results.Ok();
// });
//
// app.MapGet("service3th2/{value}", async (
//     [FromRoute] string value,
//     IPublishEndpoint publishEndpoint,
//     CancellationToken cancellationToken) =>
// {
//     await publishEndpoint.Publish(new Service3th2Event { Value = value }, cancellationToken);
//     return Results.Ok();
// });


await app.RunAsync();