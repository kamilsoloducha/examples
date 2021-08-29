using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace StockService.Consumers.TestRabbit
{
    public class TestRabbitConsumer : IConsumer<Events.TestRabbit>
    {
        private readonly ILogger<TestRabbitConsumer> logger;

        public TestRabbitConsumer(ILogger<TestRabbitConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<Events.TestRabbit> context)
        {
            logger.LogInformation($"Event {nameof(TestRabbit)} has been consumed");
            return Task.CompletedTask;
        }
    }
}
