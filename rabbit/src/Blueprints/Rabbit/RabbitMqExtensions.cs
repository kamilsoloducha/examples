using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprints.Rabbit
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection ConfigureMassTransit(
            this IServiceCollection services,
             IConfiguration configuration)
        {
            var rabbitConfig = new RabbitMqConfig();
            var section = configuration.GetSection("RabbitMq");
            section.Bind(rabbitConfig);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                    });

                    config.ConfigureEndpoints(context);
                    config.MessageTopology.SetEntityNameFormatter(new ExchangeNameFormatter());
                    config.UseConsumeFilter(typeof(MyConsumeFilter<>), context);
                    config.UsePublishFilter(typeof(MyPublishFilter<>), context);
                });
            });

            return services;
        }
    }
}
