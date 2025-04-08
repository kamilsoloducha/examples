using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Blueprints.Serilog;

public static class SerilogExtensions
{
    public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithTraceIdentifier()
            .CreateLogger();

        services.AddSingleton(_ => new SerilogLoggerFactory(Log.Logger, true) as ILoggerFactory);
        return services;
    }
}