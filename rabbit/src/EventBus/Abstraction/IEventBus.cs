namespace EventBus.Abstraction;

public interface IEventBus : IEventBusPublisher, IEventBusSubscriber, IEventBusConnector
{
}