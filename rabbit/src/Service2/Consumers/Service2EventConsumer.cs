using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Blueprints.Events;

namespace Service2.Consumers;

public class Service2EventConsumer : IConsumer<Service2Event>
{
    private readonly ILogger<Service2EventConsumer> logger;

    public Service2EventConsumer(ILogger<Service2EventConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<Service2Event> context)
    {
        logger.LogInformation("Service2Event received: {message}", context.Message.Value);
        return Task.CompletedTask;
    }
}