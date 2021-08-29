using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;

namespace Api.Features.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly ILogger<DateTimeProvider> logger;

        public DateTimeProvider(ILogger<DateTimeProvider> logger)
        {
            this.logger = logger;
        }

        public async Task<DateTime> GetDate(CancellationToken cancellationToken)
        {
            logger.LogInformation("Process started: {time}", DateTime.Now);

            for (var i = 0; i < 5; i++)
            {
                await Task.Delay(1000, cancellationToken);
                logger.LogInformation("Processing... Iteration: {iteration}", i);
            }
            return DateTime.Now;
        }
    }
}
