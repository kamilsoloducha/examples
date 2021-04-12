using System.Diagnostics;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Blueprints.Rabbit
{
    public class MyConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly ILogger<MyConsumeFilter<T>> logger;
        private readonly IServiceIdentificator identificator;

        public MyConsumeFilter(ILogger<MyConsumeFilter<T>> logger,
         IServiceIdentificator identificator)
        {
            this.logger = logger;
            this.identificator = identificator;
        }

        public void Probe(ProbeContext context)
        {
            logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Probe - Start");
            logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Probe - Stop");
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {

            var tryGetTraceId = context.Headers.TryGetHeader("TraceId", out object tractIdObj);
            if (!tryGetTraceId)
            {
                logger.LogInformation("There is no TraceId in headers");
                await next.Send(context);
            }
            var traceId = tractIdObj as string;

            // add TraceId property to logs
            using (LogContext.PushProperty("TraceId", traceId))
            {
                var currentActivity = Activity.Current;
                try
                {
                    // create new activity - in publishFilter I user AcitvityCurrent to get TraceId to track events
                    if (currentActivity == null)
                    {
                        currentActivity = new Activity("new-actitiy")
                            .SetParentId(ActivityTraceId.CreateFromString(traceId), ActivitySpanId.CreateRandom())
                            .Start();
                    }
                    logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Send - Start");
                    await next.Send(context);
                    logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Send - Stop");
                }
                finally
                {
                    currentActivity.Stop();
                }
            }


        }
    }
}
