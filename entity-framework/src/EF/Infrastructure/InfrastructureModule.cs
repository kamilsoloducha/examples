using EF.Infrastructure.Data;
using EF.Infrastructure.Settigns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Infrastructure;

public static class InfrastructureModule
{

    public static IServiceCollection AddInfrastructureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConnectionConfiguration>(options => configuration.GetSection("DatabaseConnection").Bind(options));
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<UserContext>();
        services.AddScoped<UserRepository>();
        return services;
    }
}