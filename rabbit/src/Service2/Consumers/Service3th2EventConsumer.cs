using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Blueprints.Events;

namespace Service2.Consumers;

public class Service3th2EventConsumer : IConsumer<Service3th2Event>
{
    private readonly ILogger<Service3th2EventConsumer> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public Service3th2EventConsumer(ILogger<Service3th2EventConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        this.logger = logger;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<Service3th2Event> context)
    {
        logger.LogInformation("Service3th2Event received: {message}", context.Message.Value);
        await publishEndpoint.Publish(new Service3From2Event { Value = context.Message.Value });
    }
}