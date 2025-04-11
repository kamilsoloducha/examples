using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Blueprints.RabbitClient.Abstraction;
using Serilog;

namespace Blueprints.RabbitClient.Implementation;

public class PerformenceLogDecorator<TEvent> : IEventHandler<TEvent>
{
    private readonly IEventHandler<TEvent> _decorated;

    public PerformenceLogDecorator(IEventHandler<TEvent> decorated)
    {
        _decorated = decorated;
    }
    
    public Task Handle(TEvent @event, CancellationToken cancellationToken = default)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            return _decorated.Handle(@event, cancellationToken);
        }
        finally
        {
            Log.Information("Event {Event} handled in {ElapsedMilliseconds}ms", @event.GetType().Name, stopWatch.ElapsedMilliseconds);
        }
    }
}