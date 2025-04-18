using EventBus.ErrorHandling;

namespace EventBus.Implementation;

public class EmptyFatalExceptionChecker : IFatalExceptionChecker
{
    public bool IsFatal(Exception exception) => false;
}