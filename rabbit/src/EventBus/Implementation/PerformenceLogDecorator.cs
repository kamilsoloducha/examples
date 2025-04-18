using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using EventBus.Abstraction;
using Serilog;

namespace EventBus.Implementation;

public class PerformenceLogDecorator<TEvent> : IEventHandler<TEvent>
{
    private readonly IEventHandler<TEvent> _decorated;

    public PerformenceLogDecorator(IEventHandler<TEvent> decorated)
    {
        _decorated = decorated;
    }
    
    public Task Handle([NotNull] TEvent @event, CancellationToken cancellationToken = default)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            return _decorated.Handle(@event, cancellationToken);
        }
        finally
        {
            Log.Information("Event {Event} handled in {ElapsedMilliseconds}ms", typeof(TEvent).Name, stopWatch.ElapsedMilliseconds);
        }
    }
}