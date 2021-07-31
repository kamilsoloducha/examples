using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace StockService.Configuration
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            var config = new ServiceCollectionBusConfigurator(services);
            config.AddConsumer<TestRabbitConsumer>(typeof(TestRabbitDefinition)).Endpoint(e =>
            {
                e.Name = $"stock-{nameof(Events.TestRabbit)}";
            });
            return services;
        }
    }
}