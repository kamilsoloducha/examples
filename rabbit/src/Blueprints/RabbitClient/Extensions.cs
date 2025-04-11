using System;
using System.Threading;
using System.Threading.Tasks;
using Blueprints.RabbitClient.Abstraction;
using Blueprints.RabbitClient.Configuration;
using Blueprints.RabbitClient.ErrorHandling;
using Blueprints.RabbitClient.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprints.RabbitClient;

public static class Extensions
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration,
        Func<IEventBus,CancellationToken, Task> eventBusConfig)
    {
        services.Configure<RabbitConfiguration>(configuration.GetSection(nameof(RabbitConfiguration)));
        services.AddOptions<RabbitConfiguration>()
            .Bind(configuration.GetSection(nameof(RabbitConfiguration)))
            .ValidateOnStart();
        services.AddSingleton<INamingFormatter, DefaultNamingFormatter>();
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
        services.AddSingleton<IEventBusConnector>(collection => collection.GetRequiredService<IEventBus>());
        services.AddSingleton<IEventBusPublisher>(collection => collection.GetRequiredService<IEventBus>());
        services.AddSingleton<IEventBusSubscriber>(collection => collection.GetRequiredService<IEventBus>());
        services.AddSingleton(_ => new EventBusConsumerCollection(eventBusConfig));
        services.AddSingleton(TimeProvider.System);
        services.AddHostedService<EventBusHostedService>();
    }
    
    public static IServiceCollection AddDecoratedEventHandler<TEvent, TEventHandler>(this IServiceCollection services) 
        where TEvent : class 
        where TEventHandler : class, IEventHandler<TEvent>
    {
        services.AddScoped<TEventHandler>();
        services.AddScoped<IEventHandler<TEvent>>(collection =>
        {
            var handler =  new ErrorHandlingDecorator<TEvent>(
                collection.GetRequiredService<TEventHandler>(),
                collection.GetRequiredService<IFatalExceptionChecker>()
            );
            return new PerformenceLogDecorator<TEvent>(handler);
        });
        
        return services;
    }
}