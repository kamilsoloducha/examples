using RabbitMQ.Client;

namespace EventBus.Helpers;

public static class BasicPropertiesExtensions
{
    public const string DateTimeOffsetHeaderKey = "DateTime";
    public const string DateTimeOffsetFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public const string TraceIdHeaderKey = "TraceId";
    
    public const string DelayHeaderKey = "x-delay";

    public static void AddDateTimeOffset(this IBasicProperties basicProperties, DateTime dateTimeOffset)
    {
        var dateTimeString = dateTimeOffset.ToUniversalTime().ToString(DateTimeOffsetFormat);
        basicProperties.Headers!.Add(DateTimeOffsetHeaderKey, dateTimeString);
    }
    
    public static void AddTraceId(this IBasicProperties basicProperties, string traceId)
    {
        basicProperties.Headers!.Add(TraceIdHeaderKey, traceId);
    }

    public static void AddDelay(this IBasicProperties basicProperties, TimeSpan delay)
    {
        basicProperties.Headers!.Add(DelayHeaderKey, Convert.ToInt32(Math.Abs(delay.TotalMilliseconds)));
    }

}