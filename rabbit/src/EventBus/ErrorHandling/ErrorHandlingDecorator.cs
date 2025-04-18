using System.Diagnostics.CodeAnalysis;
using EventBus.Abstraction;

namespace EventBus.ErrorHandling;

public class ErrorHandlingDecorator<TEvent> : IEventHandler<TEvent>
{
    private readonly IEventHandler<TEvent> _decorated;
    private readonly IFatalExceptionChecker _checker;

    public ErrorHandlingDecorator(IEventHandler<TEvent> handler, IFatalExceptionChecker checker)
    {
        _decorated = handler;
        _checker = checker;
    }
    
    public Task Handle([NotNull] TEvent @event, CancellationToken cancellationToken = default)
    {
        try
        {
            return _decorated.Handle(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            if (_checker.IsFatal(ex))
            {
                throw new FatalException(ex);
            }
            
            throw;
        }
    }
}