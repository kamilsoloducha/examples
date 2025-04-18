using EventBus.Abstraction;

namespace EventBus.Configuration;

public sealed class EventBusConsumerCollection
{
    public Func<IEventBus, CancellationToken, Task> ConfigAction { get; }

    public EventBusConsumerCollection(Func<IEventBus, CancellationToken, Task> configAction)
    {
        ConfigAction = configAction;
    }
}