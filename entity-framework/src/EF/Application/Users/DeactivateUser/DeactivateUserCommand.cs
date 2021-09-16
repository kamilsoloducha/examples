using System;
using MediatR;

namespace EF.Application.Users.DeactivateUser
{
    public class DeactivateUserCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}