using Blueprints.Events;
using MassTransit;
using Service2.Consumers;
using Service2.Consumers.Definitions;

namespace Service2.Configuration;

public static class MassTransitExtensions
{
    public static void DefineConsumers(this IBusRegistrationConfigurator builder)
    {
        builder.AddConsumer<Service2and3EventConsumer>(typeof(Service2and3Definition)).Endpoint(e =>
        {
            e.Name = $"service2-{nameof(Service2and3Event)}";
        });
        builder.AddConsumer<Service2EventConsumer>(typeof(Service2Definition)).Endpoint(e =>
        {
            e.Name = $"service2-{nameof(Service2Event)}";
        });
        builder.AddConsumer<Service3th2EventConsumer>(typeof(Service3th2Definition)).Endpoint(e =>
        {
            e.Name = $"service2-{nameof(Service3th2Event)}";
        });
    }
}