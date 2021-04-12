using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Blueprints
{
    public static class SerilogExtensions
    {
        public static IServiceCollection ConfigurateSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSingleton(s => new SerilogLoggerFactory(Log.Logger, true) as ILoggerFactory);
            return services;
        }
    }
}
