using System.Threading;
using System.Threading.Tasks;

namespace Blueprints.RabbitClient.Abstraction;

public interface IEventHandler<in TEvent>
{
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}