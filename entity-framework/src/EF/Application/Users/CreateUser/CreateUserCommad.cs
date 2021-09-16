using System;
using MediatR;

namespace EF.Application.Users.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}