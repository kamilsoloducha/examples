using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace NoSql.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddSingleton(new MongoClient("mongodb://root:password123@localhost:27017"));
            return services;
        }
    }
}