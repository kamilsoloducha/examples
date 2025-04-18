using EventBus.Abstraction;
using EventBus.Configuration;
using EventBus.ErrorHandling;
using EventBus.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

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