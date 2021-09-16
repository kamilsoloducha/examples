using System.Threading;
using System.Threading.Tasks;
using EF.Infrastructure.Domain;
using MediatR;

namespace EF.Application.Users.DeactivateUser
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Unit>
    {
        private readonly UserRepository _userRepository;

        public DeactivateUserCommandHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id, cancellationToken);
            user.Deactivate();

            await _userRepository.Update(user, cancellationToken);
            return Unit.Value;
        }
    }
}