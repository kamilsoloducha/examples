using System;

namespace Blueprints.RabbitClient.ErrorHandling;

public interface IFatalExceptionChecker
{
    bool IsFatal(Exception exception);
}