using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Blueprints.Events;

namespace Service2.Consumers
{
    public class Service2and3EventConsumer : IConsumer<Service2and3Event>
    {
        private readonly ILogger<Service3th2EventConsumer> logger;

        public Service2and3EventConsumer(ILogger<Service3th2EventConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<Service2and3Event> context)
        {
            logger.LogInformation("Service2and3Event received: {message}", context.Message.Value);
            return Task.CompletedTask;
        }
    }
}