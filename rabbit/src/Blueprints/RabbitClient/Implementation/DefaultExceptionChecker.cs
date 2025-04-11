using System;
using System.Linq;
using Blueprints.RabbitClient.ErrorHandling;

namespace Blueprints.RabbitClient.Implementation;

public class DefaultExceptionChecker : IFatalExceptionChecker
{
    private readonly Type[] _fatalExceptionTypes;

    public DefaultExceptionChecker(params Type[] fatalExceptionTypes)
    {
        _fatalExceptionTypes = fatalExceptionTypes;
    }
    
    public bool IsFatal(Exception exception) => _fatalExceptionTypes.Contains(exception.GetType());
}