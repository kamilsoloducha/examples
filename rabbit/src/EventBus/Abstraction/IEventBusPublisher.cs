namespace EventBus.Abstraction;

public interface IEventBusPublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}