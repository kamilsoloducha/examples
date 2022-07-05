using EF.Domain;
using EF.Infrastructure.Settigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EF.Infrastructure.Data;
public class UserContext : DbContext
{
    private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole((options) => { })
            .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information);
    });

    private readonly string _connectionString;

    public DbSet<User> Users { get; set; }

    public UserContext(IOptions<DatabaseConnectionConfiguration> databaseConnectionConfig)
    {
        _connectionString = CreateConnectionString(databaseConnectionConfig.Value);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging()
            .UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());
    }

    private static string CreateConnectionString(DatabaseConnectionConfiguration config)
        => $"Host={config.Server};Port={config.Port};Database={config.Database};User Id={config.User};Password={config.Password};";
}