using System.Threading;
using System.Threading.Tasks;

namespace Blueprints.RabbitClient.Abstraction;

public interface IEventBusConnector
{
    internal Task Connect(CancellationToken cancellationToken = default);
    internal Task Disconnect(CancellationToken cancellationToken = default);
}