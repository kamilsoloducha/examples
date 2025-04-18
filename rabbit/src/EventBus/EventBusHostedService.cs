using EventBus.Abstraction;
using EventBus.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EventBus;

internal class EventBusHostedService : IHostedService
{
    private readonly IEventBus _eventBus;
    private readonly EventBusConsumerCollection _consumerCollection;

    public EventBusHostedService(IEventBus eventBus,
        EventBusConsumerCollection consumerCollection,
        IHostApplicationLifetime applicationLifetime)
    {
        _eventBus = eventBus;
        _consumerCollection = consumerCollection;
        // applicationLifetime.ApplicationStopping.Register((_, cancellationToken) => StopAsync(cancellationToken).Wait(), null);
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information("Starting Rabbit Service");
        await _eventBus.Connect(cancellationToken);
        await _consumerCollection.ConfigAction(_eventBus, cancellationToken);
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Information("Stopping Rabbit Service");
        return _eventBus.Disconnect(cancellationToken);
    }
}