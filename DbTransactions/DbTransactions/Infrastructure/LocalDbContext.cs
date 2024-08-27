using DbTransactions.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace DbTransactions.Infrastructure;

internal class LocalDbContext : DbContext
{
    private readonly DatabaseConfiguration _configuration;
    
    public DbSet<Transfer> Transfers { get; init; }
    
    public LocalDbContext(DbContextOptions options, IOptions<DatabaseConfiguration> configuration) : base(options)
    {
        _configuration = configuration.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        switch (_configuration.DatabaseType)
        {
            case DatabaseType.Postgres:
                optionsBuilder.UseNpgsql(_configuration.ConnectionString);
                break;
            case DatabaseType.Mysql:
                optionsBuilder.UseMySQL(_configuration.ConnectionString);
                break;
            case DatabaseType.Sqlserver:
                optionsBuilder.UseSqlServer(_configuration.ConnectionString);
                break;
            case DatabaseType.Oracle:
                optionsBuilder.UseOracle(_configuration.ConnectionString);
                break;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transfer>(b =>
        {
            b.ToTable("transfers");
            b.HasKey(x => x.ContractNumber);

            b.Property(x => x.ContractNumber).ValueGeneratedOnAdd();
            b.Property(x => x.SourceAccount).IsRequired();
            b.Property(x => x.DestinationAccount).IsRequired();
            b.Property(x => x.Status).IsRequired().HasConversion(new EnumToStringConverter<TransferStatus>());
            b.Property(x => x.Shares).IsRequired();
        });
    }
}
