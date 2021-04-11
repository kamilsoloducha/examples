using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Blueprints.Events;

namespace Service3.Consumers
{
    public class Service2and3EventConsumer : IConsumer<Service2and3Event>
    {
        private readonly ILogger<Service2and3EventConsumer> logger;

        public Service2and3EventConsumer(ILogger<Service2and3EventConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<Service2and3Event> context)
        {
            logger.LogInformation("CommonEvent3 received: {message}", context.Message.Value);
            return Task.CompletedTask;
        }
    }
}