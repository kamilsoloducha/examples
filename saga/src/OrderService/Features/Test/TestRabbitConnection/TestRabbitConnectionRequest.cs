using MediatR;

namespace OrderService.Features.Test.TestRabbitConnection
{
    public class TestRabbitConnectionRequest : IRequest
    {
        public string Message { get; set; }
    }
}