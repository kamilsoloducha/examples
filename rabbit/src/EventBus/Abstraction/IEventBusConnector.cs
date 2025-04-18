namespace EventBus.Abstraction;

public interface IEventBusConnector
{
    internal Task Connect(CancellationToken cancellationToken = default);
    internal Task Disconnect(CancellationToken cancellationToken = default);
}