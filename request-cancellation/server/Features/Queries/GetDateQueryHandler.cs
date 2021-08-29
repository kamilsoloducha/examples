using System.Threading.Tasks;
using MediatR;
using System.Threading;
using System;
using Api.Features.Providers;

namespace Api.Features.Queries
{
    public class GetDateQueryHandler : IRequestHandler<GetDateQuery, DateTime>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public GetDateQueryHandler(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<DateTime> Handle(GetDateQuery request, CancellationToken cancellationToken)
        => await dateTimeProvider.GetDate(cancellationToken);
    }
}

