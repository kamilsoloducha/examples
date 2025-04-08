using System;
using Serilog;
using Serilog.Configuration;

namespace Blueprints.Serilog;

public static class TraceEnricherConfigurationExtensions
{
    public static LoggerConfiguration WithTraceIdentifier(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<TraceEnricher>();
    }
}