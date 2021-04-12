using System.Diagnostics;
using Blueprints;
using Blueprints.Events;
using Blueprints.Rabbit;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servic3.Services;
using Service3.Consumers;
using Service3.Consumers.Definitions;

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
            services.ConfigureMassTransit(Configuration);
            services.ConfigurateSerilog(Configuration);
            services.AddSingleton<IServiceIdentificator, Service3Identificator>();



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
}
