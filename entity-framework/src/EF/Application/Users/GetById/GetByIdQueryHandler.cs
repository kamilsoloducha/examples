using System.Threading;
using System.Threading.Tasks;
using EF.Infrastructure.Data;
using MediatR;

namespace EF.Application.Users.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, GetByIdResponse>
    {
        private readonly UserRepository _userRepository;

        public GetByIdQueryHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetByIdResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id, cancellationToken);
            return new GetByIdResponse
            {
                Id = user.Id,
                Name = user.Name,
                IsActive = user.IsActive
            };
        }
    }
}