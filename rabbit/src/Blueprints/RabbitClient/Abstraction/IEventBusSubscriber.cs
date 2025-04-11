using System.Threading;
using System.Threading.Tasks;

namespace Blueprints.RabbitClient.Abstraction;

public interface IEventBusSubscriber
{
    Task Subscribe<TEvent>(CancellationToken cancellationToken = default);
}