using System;
using Blueprints.RabbitClient.ErrorHandling;

namespace Blueprints.RabbitClient.Implementation;

public class EmptyFatalExceptionChecker : IFatalExceptionChecker
{
    public bool IsFatal(Exception exception) => false;
}