using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Service;

public static class CorsExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Delete", builder => builder
                .WithMethods("DELETE", "POST")
                .WithHeaders("testheader")
                .WithOrigins("http://localhost:4200")
            );
        });
        return services;
    }

    public static void UseCustomCors(this WebApplication app)
    {
        // app.UseCors("Get");
        app.UseCors("Post");
        app.UseCors("Delete");
        

        // app.UseCors(builder =>
        // {
        //     builder.WithMethods("Get");
        //     builder.WithOrigins("http://localhost:4200");
        // });
    }
}