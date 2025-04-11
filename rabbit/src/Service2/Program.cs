using Blueprints.Events;
using Blueprints.RabbitClient;
using Blueprints.RabbitMassTransit;
using Blueprints.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Servic2.Services;
using Service2.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.ConfigureMassTransit(configuration, MassTransitExtensions.DefineConsumers);
builder.Services.AddRabbitMq(configuration, async (bus, token) =>
{
    await bus.Subscribe<Service2Event>(token);
});
builder.Services.ConfigureSerilog(configuration);
builder.Services.AddSingleton<IServiceIdentificator, Service2Identificator>();
builder.Services.AddHostedService<BusService>();

var app = builder.Build();

await app.RunAsync();