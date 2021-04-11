using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Blueprints;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service1.Services;
using Microsoft.OpenApi.Models;
using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;

namespace Service1
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
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RabbitMq Testing",
                    Description = "Api to testing rabbitmq and masstransit"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddControllers();

            var rabbitConfig = new Blueprints.RabbitMqConfig();
            var section = Configuration.GetSection("RabbitMq");
            section.Bind(rabbitConfig);

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitConfig.Host, rabbitConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                    });

                    config.MessageTopology.SetEntityNameFormatter(new Blueprints.ExchangeNameFormatter());
                    config.UsePublishFilter(typeof(Blueprints.MyPublishFilter<>), context);
                });
            });

            services.AddMassTransitHostedService();
            services.AddSingleton<IServiceIdentificator, Service1Identificator>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app
            .UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            })
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RabbitMq testing api");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
