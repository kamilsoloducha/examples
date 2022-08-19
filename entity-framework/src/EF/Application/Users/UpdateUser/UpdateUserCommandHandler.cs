using System;
using System.Threading;
using System.Threading.Tasks;
using EF.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EF.Application.Users.UpdateUser;

internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ILogger<UpdateUserCommand> _logger;
    private readonly UserRepository _userRepository;

    public UpdateUserCommandHandler(
        UserRepository userRepository,
         ILogger<UpdateUserCommand> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var eventId = new EventId(new Random().Next());
        _logger.LogInformation(eventId, "New EventId {EventId}", eventId.Id);
        using var transaction = await _userRepository.OpenTransaction(cancellationToken);
        _logger.LogInformation(eventId, "Transaction opened");

        var user = await _userRepository.GetById(request.Id, cancellationToken);
        _logger.LogInformation(eventId, "User {UserId} with age {Age}", user.Id, user.Age);

        _logger.LogInformation(eventId, "Waiting 5s");
        await Task.Delay(5000);

        user.Age = request.Age;

        _logger.LogInformation(eventId, "Updating database - {UserId} - {Age}", user.Id, user.Age);
        await _userRepository.Update(user, cancellationToken);

        _logger.LogInformation(eventId, "Commiting changes");
        await transaction.CommitAsync();
        return Unit.Value;
    }

}