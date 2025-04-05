using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server;
using Server.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisismysupersecretpasswordthisismysupersecretpassword")),
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

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services
    .AddSignalR();
builder.Services.AddTransient<IConnectionStore, MessageHub>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();
app.UseCors("CorsPolicy");
app.MapHub<MessageHub>("/notify");
app.AddAuthenticate()
    .AddGetUser()
    .AddMessageSend();

await app.RunAsync();