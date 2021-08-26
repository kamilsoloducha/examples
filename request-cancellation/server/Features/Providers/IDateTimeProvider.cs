using System.Threading.Tasks;
using System.Threading;
using System;

namespace Api.Features.Providers
{
    public interface IDateTimeProvider
    {
        Task<DateTime> GetDate(CancellationToken cancellationToken);
    }
}
