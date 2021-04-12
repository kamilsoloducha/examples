using Blueprints.Events;
using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.Extensions.DependencyInjection;
using Service2.Consumers;
using Service2.Consumers.Definitions;

namespace Servic2.Configuration
{
    public static class MassTransitExtensions
    {

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            var config = new ServiceCollectionBusConfigurator(services);
            config.AddConsumer<Service2and3EventConsumer>(typeof(Service2and3Definition)).Endpoint(e =>
            {
                e.Name = $"service2-{nameof(Service2and3Event)}";
            });
            config.AddConsumer<Service2EventConsumer>(typeof(Service2Definition)).Endpoint(e =>
            {
                e.Name = $"service2-{nameof(Service2Event)}";
            });
            config.AddConsumer<Service3th2EventConsumer>(typeof(Service3th2Definition)).Endpoint(e =>
            {
                e.Name = $"service2-{nameof(Service3th2Event)}";
            });
            return services;
        }
    }
}