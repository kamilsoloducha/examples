using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstraction;
using Serilog;

namespace Service1.EventHandlers;

internal sealed class Process2FinishedHandler : IEventHandler<Process2Finished>
{
    public Task Handle(Process2Finished @event, CancellationToken cancellationToken = default)
    {
        Log.Information("Process2FinishedHandler started - {@Event}", @event);
        
        Log.Information("Process2FinishedHandler finished");
        
        return Task.CompletedTask;
    }
}