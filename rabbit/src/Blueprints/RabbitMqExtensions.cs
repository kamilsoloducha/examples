using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Topology;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blueprints
{
    public class RabbitMqConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddMassTransit(
            this IServiceCollection services,
             IConfiguration configuration,
             Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator, IServiceCollectionBusConfigurator> configAction = null)
        {
            var rabbitConfig = new RabbitMqConfig();
            var section = configuration.GetSection("RabbitMq");
            section.Bind(rabbitConfig);

            services.AddMassTransit(x =>
            {
                // x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                    });
                    // config.UseJsonSerializer();
                    // config.ConfigureEndpoints(context);
                    config.MessageTopology.SetEntityNameFormatter(new ExchangeNameFormatter());
                    // if (configAction != null)
                    //     configAction(context, config, x);
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }

    public class ExchangeNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
        {
            var name = typeof(T).Name;
            return $"{name}_Exchange";
        }
    }

    public class BusService : IHostedService
    {
        private readonly IBusControl busControl;

        public BusService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken) => await busControl.StartAsync(cancellationToken);

        public async Task StopAsync(CancellationToken cancellationToken) => await busControl.StopAsync(cancellationToken);
    }

    public class MyConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly ILogger<MyConsumeFilter<T>> logger;
        private readonly IServiceIdentificator identificator;

        public MyConsumeFilter(ILogger<MyConsumeFilter<T>> logger, IServiceIdentificator identificator)
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
            logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Send - Start");
            var tryGetTraceId = context.Headers.TryGetHeader("TraceId", out object tractIdObj);
            if (!tryGetTraceId)
            {
                logger.LogInformation("There is no TraceId in headers");
                await next.Send(context);
            }
            var traceId = tractIdObj as string;
            var currentActivity = Activity.Current;
            try
            {
                if (currentActivity == null)
                {
                    currentActivity = new Activity("new-actitiy")
                        .SetParentId(ActivityTraceId.CreateFromString(traceId), ActivitySpanId.CreateRandom())
                        .Start();
                }
                await next.Send(context);
            }
            finally
            {
                currentActivity.Stop();
            }

            logger.LogInformation($"{identificator.Id} - {typeof(MyConsumeFilter<>).FullName} - {typeof(T)} - Send - Stop");
        }
    }

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

            context.Headers.Set("TraceId", Activity.Current.TraceId.ToHexString());
            await next.Send(context);

            logger.LogInformation($"{identificator.Id} - {typeof(MyPublishFilter<>).FullName} - {typeof(T)} - Send - Stop");
        }
    }

    public interface IServiceIdentificator
    {
        string Id { get; }
    }
}
