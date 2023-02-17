using EfCoreDdd.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDdd.Infrastructure.DataAccess;

public class LocalDbContext : DbContext
{
    private readonly bool _isProduction = false;
    public const string Schema = "ddd";
    public DbSet<Owner> Owners { get; private set; }
    public DbSet<Group> Groups { get; private set; }

    public LocalDbContext(IHostEnvironment hostEnvironment)
    {
        _isProduction = hostEnvironment.IsProduction();
    }

    public LocalDbContext(bool isProduction)
    {
        _isProduction = isProduction;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseNpgsql("User ID=root;Password=changeme;Host=localhost;Port=5432;Database=myDataBase;");

        if (!_isProduction)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocalDbContext).Assembly);
    }
}