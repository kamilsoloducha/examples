using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blueprints.Rabbit;

public class MyPublishFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly ILogger<MyPublishFilter<T>> logger;
    private readonly IServiceIdentificator identificator;

    public MyPublishFilter(ILogger<MyPublishFilter<T>> logger, IServiceIdentificator identificator)
    {
        this.logger = logger;
        this.identificator = identificator;
    }

    public void Probe(ProbeContext context)
    {
        logger.LogInformation($"{identificator.Id} - {typeof(MyPublishFilter<>).FullName} - {typeof(T)} - Probe - Start");
        logger.LogInformation($"{identificator.Id} - {typeof(MyPublishFilter<>).FullName} - {typeof(T)} - Probe - Stop");
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        logger.LogInformation($"{identificator.Id} - {typeof(MyPublishFilter<>).FullName} - {typeof(T)} - Send - Start");

        // get TraceId from Activity.Currect - it was setup In ConsumeFilter in case consume an event,
        // or it origins from httpRequest in case common a http request
        context.Headers.Set("TraceId", Activity.Current.TraceId.ToHexString());
        await next.Send(context);

        logger.LogInformation($"{identificator.Id} - {typeof(MyPublishFilter<>).FullName} - {typeof(T)} - Send - Stop");
    }
}