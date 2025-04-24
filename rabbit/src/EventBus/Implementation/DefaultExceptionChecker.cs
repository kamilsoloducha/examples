using EventBus.ErrorHandling;
using RabbitMQ.Client.Events;

namespace EventBus.Implementation;

public class DefaultExceptionChecker : IFatalExceptionChecker
{
    private readonly Type[] _fatalExceptionTypes;

    public DefaultExceptionChecker(params Type[] fatalExceptionTypes)
    {
        _fatalExceptionTypes = fatalExceptionTypes;
    }
    
    public bool IsFatal(Exception exception) => _fatalExceptionTypes.Contains(exception.GetType());
}


public class NoBackoffHandler : IBackoffHandler
{
    public Task Handle<TEvent>(BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}