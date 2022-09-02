using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/get/{value}",
    ([FromRoute] string value, [FromServices] ILogger<Program> logger) => { logger.LogInformation("Get - {Value}", value); });

app.MapPost("/post",
    ([FromBody] string value, [FromServices] ILogger<Program> logger) => { logger.LogInformation("Post - {Value}", value); });

app.MapDelete("/delete/{value}",
    ([FromRoute] string value, [FromServices] ILogger<Program> logger) => { logger.LogInformation("Delete - {Value}", value); });

app.MapPut("/put",
    ([FromBody] string value, [FromServices] ILogger<Program> logger) => { logger.LogInformation("Put - {Value}", value); });

app.Run();