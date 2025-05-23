using Blueprints.RabbitMassTransit;
using Blueprints.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Servic3.Services;
using Service3.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.ConfigureMassTransit(configuration, MassTransitExtensions.DefineConsumers);
builder.Services.ConfigureSerilog(configuration);
builder.Services.AddSingleton<IServiceIdentificator, Service3Identificator>();
builder.Services.AddHostedService<BusService>();

var app = builder.Build();

await app.RunAsync();