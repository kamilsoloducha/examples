using RabbitMQ.Client.Events;

namespace EventBus.ErrorHandling;

public interface IBackoffHandler
{
    Task Handle<TEvent>(BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken);
}