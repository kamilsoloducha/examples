using Blueprints.Internal;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Blueprints;

public static class ConsulExtensions
{
    public static IServiceCollection AddConsul(this IServiceCollection services)
    {
        IConfiguration configuration;

        using (var serviceProvider = services.BuildServiceProvider())
        {
            configuration = serviceProvider.GetService<IConfiguration>();
        }

        services.Configure<ConsulConfiguration>(options =>
            configuration.GetSection(nameof(ConsulConfiguration)).Bind(options));

        var consulConfiguration = configuration.GetOptions<ConsulConfiguration>();

        services.AddScoped<IServiceDiscoveryUrlProvider, ConsulUrlProvider>();
        services.AddSingleton<IServiceIdProvider<Guid>, GuidServiceIdProvider>();
        services.AddSingleton<IConsulClient>(_ => new ConsulClient(config =>
        {
            if (!string.IsNullOrEmpty(consulConfiguration.Url))
            {
                try
                {
                    config.Address = new Uri(consulConfiguration.Url);
                }
                catch (Exception e)
                {
                    throw new Exception(consulConfiguration.Url, e);
                }
                
            }
        }));

        return services;
    }

    public static void UseConsul(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var consulOptions = scope.ServiceProvider.GetService<IOptions<ConsulConfiguration>>();
        ArgumentNullException.ThrowIfNull(consulOptions);
        
        var applicationLifeTime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
        ArgumentNullException.ThrowIfNull(applicationLifeTime);
        
        var serviceIdProvider = scope.ServiceProvider.GetService<IServiceIdProvider<Guid>>();
        ArgumentNullException.ThrowIfNull(serviceIdProvider);

        if (!consulOptions.Value.Enabled)
        {
            return;
        }

        var client = scope.ServiceProvider.GetService<IConsulClient>();
        ArgumentNullException.ThrowIfNull(client);

        var healthCheckInterval = consulOptions.Value.PingInterval <= 0 ? 5 : consulOptions.Value.PingInterval;
        var healthCheckRemovingTimeOut =
            consulOptions.Value.RemoveAfterInterval <= 0 ? 5 : consulOptions.Value.RemoveAfterInterval;
        var address = consulOptions.Value.Address;

        var port = consulOptions.Value.Port;
        var pingEndpoint = consulOptions.Value.PingEndpoint;

        var scheme = address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
            ? string.Empty
            : "http://";

        var healthCheck = new AgentServiceCheck
        {
            Interval = TimeSpan.FromSeconds(healthCheckInterval),
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(healthCheckRemovingTimeOut),
            HTTP = $"{scheme}{address}:{port}/{pingEndpoint}"
        };

        var serviceName = consulOptions.Value.Service;
        var serviceId = serviceIdProvider.GetId().ToString();

        var registration = new AgentServiceRegistration
        {
            Name = serviceName,
            Port = port,
            Address = address,
            ID = serviceId,
            Checks = new[] { healthCheck }
        };
        client.Agent.ServiceRegister(registration);

        applicationLifeTime.ApplicationStopped.Register(() =>
        {
            client.Agent.ServiceDeregister(serviceId);
        });
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName = null) where T : class, new()
    {
        var key = sectionName ?? typeof(T).Name;
        T options = new();

        configuration.GetSection(key).Bind(options);
        return options;
    }
}