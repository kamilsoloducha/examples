using System.Threading;
using System.Threading.Tasks;
using Events;
using MassTransit;
using MediatR;

namespace Feature.Test.TestRabbitConnection
{
    public class TestRabbitConnectionRequestHandler : IRequestHandler<TestRabbitConnectionRequest>
    {
        private readonly IPublishEndpoint publisher;

        public TestRabbitConnectionRequestHandler(IPublishEndpoint publisher)
        {
            this.publisher = publisher;
        }

        public async Task<Unit> Handle(TestRabbitConnectionRequest request, CancellationToken cancellationToken)
        {
            await publisher.Publish(new Events.TestRabbit { Message = request.Message }, cancellationToken);
            return Unit.Value;
        }
    }
}