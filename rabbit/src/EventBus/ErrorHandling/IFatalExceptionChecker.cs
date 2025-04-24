using RabbitMQ.Client;

namespace EventBus.ErrorHandling;

public interface IFatalExceptionChecker
{
    bool IsFatal(Exception exception);
}