using System.Threading;
using System.Threading.Tasks;

namespace Blueprints.RabbitClient.Abstraction;

public interface IEventBusPublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}