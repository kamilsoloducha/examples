using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Blueprints;

public class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (Activity.Current == null)
            return;
        var traceId = propertyFactory.CreateProperty("TraceId", Activity.Current.TraceId);
        logEvent.AddOrUpdateProperty(traceId);
        var SpanId = propertyFactory.CreateProperty("SpanId", Activity.Current.SpanId);
        logEvent.AddOrUpdateProperty(SpanId);
    }
}