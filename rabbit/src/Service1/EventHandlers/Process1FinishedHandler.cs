using System.Threading;
using System.Threading.Tasks;
using Blueprints.RabbitClient.Abstraction;
using Serilog;
using Service1.EventHandlers.Events;

namespace Service1.EventHandlers;

internal sealed class Process1FinishedHandler : IEventHandler<Process1Finished>
{
    public async Task Handle(Process1Finished @event, CancellationToken cancellationToken = default)
    {
        Log.Information("Process1FinishedHandler started - {@Event}", @event);
        await Task.Delay(20000, cancellationToken);
        Log.Information("Process1FinishedHandler finished");
    }
}