using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading;
using Api.Features.Queries;

namespace Api.Features
{
    [ApiController]
    [Route("[controller]")]
    public class DateController : ControllerBase
    {
        private readonly IMediator mediator;

        public DateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("ct")]
        public async Task<IActionResult> GetDateWithCt(CancellationToken cancellationToken)
            => new JsonResult(await mediator.Send(new GetDateQuery(), cancellationToken));

        [HttpGet()]
        public async Task<IActionResult> GetDate()
            => new JsonResult(await mediator.Send(new GetDateQuery()));
    }

}