using System.Threading.Tasks;
using Blueprints.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Service3.Consumers;

public class Service3From2EventConsumer : IConsumer<Service3From2Event>
{
    private readonly ILogger<Service3From2EventConsumer> logger;

    public Service3From2EventConsumer(ILogger<Service3From2EventConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<Service3From2Event> context)
    {
        logger.LogInformation("Service3From2Event received: {message}", context.Message.Value);
        return Task.CompletedTask;
    }
}