using RabbitMQ.Client;

namespace EventBus.Helpers;

public static class BasicPropertiesExtensions
{
    public const string DateTimeOffsetHeaderName = "DateTime";
    public const string DateTimeOffsetFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public const string TraceIdHeaderName = "TraceId";

    public static void AddDateTimeOffset(this IBasicProperties basicProperties, DateTime dateTimeOffset)
    {
        var dateTimeString = dateTimeOffset.ToUniversalTime().ToString(DateTimeOffsetFormat);
        basicProperties.Headers!.Add(DateTimeOffsetHeaderName, dateTimeString);
    }
    
    public static void AddTraceId(this IBasicProperties basicProperties, string traceId)
    {
        basicProperties.Headers!.Add(TraceIdHeaderName, traceId);
    }

}