using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstraction;
using Serilog;
using Service1.EventHandlers.Events;

namespace Service1.EventHandlers;

internal sealed class Process1FinishedHandler : IEventHandler<Process1Finished>
{
    public async Task Handle(Process1Finished @event, CancellationToken cancellationToken = default)
    {
        Log.Information("Process1FinishedHandler started - {@Event}", @event);
        // await Task.Delay(2000, cancellationToken);
        throw new Exception();
        Log.Information("Process1FinishedHandler finished");
    }
}