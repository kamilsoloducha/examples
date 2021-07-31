using System.Threading;
using System.Threading.Tasks;
using Feature.Test.TestRabbitConnection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Feature.Test
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("rabbit/{message}")]
        public async Task<IActionResult> TestRabbitConnection(string message, CancellationToken cancelletionToken)
        {
            await mediator.Send(
                new TestRabbitConnectionRequest { Message = message },
                cancelletionToken);
            return Ok();
        }
    }
}