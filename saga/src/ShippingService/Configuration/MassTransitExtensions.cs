using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.Extensions.DependencyInjection;
using ShippingService.Consumers.TestRabbit;

namespace ShippingService.Configuration
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            var config = new ServiceCollectionBusConfigurator(services);
            config.AddConsumer<TestRabbitConsumer>(typeof(TestRabbitDefinition)).Endpoint(e =>
            {
                e.Name = $"shipping-{nameof(Events.TestRabbit)}";
            });
            return services;
        }
    }
}