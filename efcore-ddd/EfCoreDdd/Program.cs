using EfCoreDdd.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LocalDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var localDbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
localDbContext.Database.EnsureDeleted();
localDbContext.Database.EnsureCreated();
scope.Dispose();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();