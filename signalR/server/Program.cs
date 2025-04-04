using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Server;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {

        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey("thisismysupersecretpasswordthisismysupersecretpassword"u8.ToArray()),
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
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
builder.Services.AddControllers();
var redisConnectionString = builder.Configuration.GetValue<string>("RedisHost");
builder.Services.AddSignalR().AddStackExchangeRedis(redisConnectionString, options =>
{
    options.Configuration.ChannelPrefix = "MyApp";
});
builder.Services.AddTransient<IConnectionStore, MessageHub>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<MessageHub>("/notify");
app.UseWebSockets();
app.Run();