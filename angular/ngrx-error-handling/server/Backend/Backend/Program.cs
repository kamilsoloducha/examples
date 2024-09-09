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

app.MapGet("/value/{timeout}", async (int timeout) =>
    {
        await Task.Delay(TimeSpan.FromSeconds(timeout));
        return Results.Ok(DateTime.Now);
    })
    .WithOpenApi();

app.Run();