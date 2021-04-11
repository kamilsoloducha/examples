using System.Diagnostics;
using Blueprints;
using Blueprints.Events;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servic2.Services;
using Service2.Consumers;

namespace Service2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var rabbitConfig = new Blueprints.RabbitMqConfig();
            var section = Configuration.GetSection("RabbitMq");
            section.Bind(rabbitConfig);

            services.AddMassTransit(x =>
            {
                // x.AddConsumer<Service2and3EventConsumer>();
                // x.AddConsumer<Service2EventConsumer>();
                // x.AddConsumer<Service3th2EventConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                    });

                    config.MessageTopology.SetEntityNameFormatter(new Blueprints.ExchangeNameFormatter());
                    // config.ConfigureEndpoints(context);
                    config.UseConsumeFilter(typeof(MyConsumeFilter<>), context);
                    config.UsePublishFilter(typeof(MyPublishFilter<>), context);
                });
            });
            // services.AddMassTransitHostedService();
            services.AddSingleton<IServiceIdentificator, Service2Identificator>();

            var config = new ServiceCollectionBusConfigurator(services);
            config.AddConsumer<Service2and3EventConsumer>(typeof(Service2and3Definition)).Endpoint(e =>
            {
                e.Name = $"service2-{nameof(Service2and3Event)}-end";
            });

            services.AddHostedService<BusService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class Service2and3Definition : ConsumerDefinition<Service2and3EventConsumer>
    {

        public Service2and3Definition()
        {
            EndpointName = $"service2-{nameof(Service2and3Event)}-def";
        }

        // protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        // IConsumerConfigurator<Service2and3EventConsumer> consumerConfigurator)
        // {
        // }
    }
}
