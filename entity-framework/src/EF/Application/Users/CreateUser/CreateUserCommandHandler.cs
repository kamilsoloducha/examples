using System;
using System.Threading;
using System.Threading.Tasks;
using EF.Domain;
using EF.Infrastructure.Domain;
using MediatR;

namespace EF.Application.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly UserRepository _userRepository;

        public CreateUserCommandHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(request.UserName, request.Password);
            await _userRepository.AddUser(user, cancellationToken);
            return user.Id;
        }
    }
}