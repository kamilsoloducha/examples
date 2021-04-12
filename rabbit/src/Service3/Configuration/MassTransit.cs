using Blueprints.Events;
using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.Extensions.DependencyInjection;
using Service3.Consumers;
using Service3.Consumers.Definitions;

namespace Service3.Configuration
{
    public static class MassTransitExtensions
    {

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            var config = new ServiceCollectionBusConfigurator(services);
            config.AddConsumer<Service2and3EventConsumer>(typeof(Service2and3Definition)).Endpoint(e =>
            {
                e.Name = $"service3-{nameof(Service2and3Event)}";
            });
            config.AddConsumer<Service3From2EventConsumer>(typeof(Service3From2Definition)).Endpoint(e =>
            {
                e.Name = $"service3-{nameof(Service3From2Event)}";
            });
            return services;
        }
    }
}