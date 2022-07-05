using System;
using MediatR;

namespace EF.Application.Users.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public Guid Id { get; set; }
    public int Age { get; set; }
}
