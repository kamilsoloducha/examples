var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/exception/{value}", (int value) =>
    {
        throw new Exception($"Custom Error Message, {value}");
    })
    .WithName("Exception throwing")
    .WithOpenApi();

app.Run();