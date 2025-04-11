namespace Blueprints.RabbitClient.Abstraction;

public interface IEventBus : IEventBusPublisher, IEventBusSubscriber, IEventBusConnector
{
}