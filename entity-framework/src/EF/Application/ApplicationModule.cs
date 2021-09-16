using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace EF.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationModule));
            return services;
        }
    }
}