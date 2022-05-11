using System;
using System.Threading;
using System.Threading.Tasks;
using EF.Domain;
using Microsoft.EntityFrameworkCore;

namespace EF.Infrastructure.Domain
{
    public class UserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            this._userContext = userContext;
        }

        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            await _userContext.Users.AddAsync(user, cancellationToken);
            await _userContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
            => await _userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            _userContext.Update(user);
            await _userContext.SaveChangesAsync(cancellationToken);
        }
    }
}