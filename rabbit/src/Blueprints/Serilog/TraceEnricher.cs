using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Blueprints.Serilog;

public class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (Activity.Current == null)
            return;
        var traceId = propertyFactory.CreateProperty("TraceId", Activity.Current.TraceId);
        logEvent.AddOrUpdateProperty(traceId);
        var spanId = propertyFactory.CreateProperty("SpanId", Activity.Current.SpanId);
        logEvent.AddOrUpdateProperty(spanId);
        
    }
}