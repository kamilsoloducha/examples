using System;
using System.Threading;
using System.Threading.Tasks;
using Blueprints.RabbitClient.Abstraction;

namespace Blueprints.RabbitClient.Configuration;

public sealed class EventBusConsumerCollection
{
    public Func<IEventBus, CancellationToken, Task> ConfigAction { get; }

    public EventBusConsumerCollection(Func<IEventBus, CancellationToken, Task> configAction)
    {
        ConfigAction = configAction;
    }
}