using DbTransactions.Api;
using DbTransactions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection(nameof(DatabaseConfiguration)));
builder.Services.AddDbContext<LocalDbContext>();

var host = builder.Build();

host.AddTransfer()
    .UpdateStatus()
    .UpdateStatusTransaction()
    .GetTransfer();


await InitializeDatabase(host);

host.Run();


static async Task InitializeDatabase(IHost host)
{
    await using var scope = host.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}