using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Server
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(x =>
                        {

                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisismysupersecretpassword")),
                                ValidateIssuerSigningKey = true,
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                            x.RequireHttpsMetadata = false;
                            x.IncludeErrorDetails = true;

                            x.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    string accessToken = context.Request.Query["access_token"];
                                    var path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(accessToken) &&
                                        (path.StartsWithSegments("/notify")))
                                    {
                                        if (accessToken.StartsWith("Bearer"))
                                        {
                                            accessToken = accessToken.Substring("Bearer".Length + 1);
                                        }
                                        context.Token = accessToken;
                                    }
                                    return Task.CompletedTask;
                                }
                            };
                        });

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
                        {
                            builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        }));
            services.AddControllers();
            services.AddSignalR();
            services.AddTransient<IConnectionStore, MessageHub>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/notify");
            });
            app.UseWebSockets();
        }
    }
}
