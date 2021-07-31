using MediatR;

namespace Feature.Test.TestRabbitConnection
{
    public class TestRabbitConnectionRequest : IRequest
    {
        public string Message { get; set; }
    }
}