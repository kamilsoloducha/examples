namespace EventBus.ErrorHandling;

public class FatalException : Exception
{
    public FatalException(Exception innerException) : base(innerException.Message, innerException) { }
}