using System.Threading;
using System.Threading.Tasks;
using EF.Domain;
using EF.Infrastructure.Data;
using MediatR;

namespace EF.Application.Users.DeactivateUser;
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

        var user2 = new User
        {
            Id = user.Id,
            Name = user.Name,
            Password = user.Password,
            IsActive = true,
        };
        user2.Deactivate();

        await _userRepository.Update(user2, cancellationToken);
        return Unit.Value;
    }
}