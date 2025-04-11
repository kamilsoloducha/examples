using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprints.RabbitMassTransit;

public static class RabbitMqExtensions
{
    public static IServiceCollection ConfigureMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> builderAction = null)
    {
        // get configuration from appSettings file
        var rabbitConfig = new RabbitMqConfig();
        var section = configuration.GetSection("RabbitMq");
        section.Bind(rabbitConfig);

        services.AddMassTransit(x =>
        {
            builderAction?.Invoke(x);

            x.UsingRabbitMq((context, config) =>
            {
                // configure connection to rabbit host
                config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                {
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });

                config.ConfigureEndpoints(context);

                // change exchange name formatter
                config.MessageTopology.SetEntityNameFormatter(new ExchangeNameFormatter());

                // add consumer and publish filter
                config.UseConsumeFilter(typeof(MyConsumeFilter<>), context);
                config.UsePublishFilter(typeof(MyPublishFilter<>), context);
            });
        });

        return services;
    }
}