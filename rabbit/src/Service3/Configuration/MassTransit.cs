using Blueprints.Events;
using MassTransit;
using Service3.Consumers;
using Service3.Consumers.Definitions;

namespace Service3.Configuration;

public static class MassTransitExtensions
{
    public static void DefineConsumers(this IBusRegistrationConfigurator builder)
    {
        builder.AddConsumer<Service2and3EventConsumer>(typeof(Service2and3Definition)).Endpoint(e =>
        {
            e.Name = $"service3-{nameof(Service2and3Event)}";
        });
        builder.AddConsumer<Service3From2EventConsumer>(typeof(Service3From2Definition)).Endpoint(e =>
        {
            e.Name = $"service3-{nameof(Service3From2Event)}";
        });
    }
}