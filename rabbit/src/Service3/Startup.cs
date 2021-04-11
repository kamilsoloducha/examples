using System;
using System.Diagnostics;
using System.Reflection;
using Blueprints;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servic3.Services;
using Service3.Consumers;

namespace Service3
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
                x.AddConsumer<Service2and3EventConsumer>();
                x.AddConsumer<Service3From2EventConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                    });

                    config.MessageTopology.SetEntityNameFormatter(new Blueprints.ExchangeNameFormatter());
                    config.ConfigureEndpoints(context);
                    config.UseConsumeFilter(typeof(MyConsumeFilter<>), context);
                    config.UsePublishFilter(typeof(MyPublishFilter<>), context);
                });
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IServiceIdentificator, Service3Identificator>();
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
}
