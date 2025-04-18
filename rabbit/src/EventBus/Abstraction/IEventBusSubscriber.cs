namespace EventBus.Abstraction;

public interface IEventBusSubscriber
{
    Task Subscribe<TEvent>(CancellationToken cancellationToken = default);
}