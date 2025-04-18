using EventBus.ErrorHandling;

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