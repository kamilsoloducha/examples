using Microsoft.AspNetCore.Mvc;
using MediatR;
using EF.Application.Users.CreateUser;
using System.Threading;
using System.Threading.Tasks;
using System;
using EF.Application.Users.GetById;
using EF.Application.Users.DeactivateUser;

namespace EF.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
            => new JsonResult(await _mediator.Send(command, cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
            => new JsonResult(await _mediator.Send(new GetByIdQuery { Id = id }, cancellationToken));

        [HttpPut("deactivate")]
        public async Task<IActionResult> Deactivate(DeactivateUserCommand command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(command, cancellationToken));


    }
}