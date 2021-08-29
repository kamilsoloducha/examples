using Blueprints.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockService.Database.Configuration;
using StockService.Model;

namespace StockService.Database
{
    public class StockDbContext : DbContext
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ILogger<StockDbContext> logger;
        public DbSet<Item> Items { get; set; }

        public StockDbContext(IConnectionStringProvider connectionStringProvider,
        ILogger<StockDbContext> logger)
        {
            this.connectionStringProvider = connectionStringProvider;
            this.logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = connectionStringProvider.GetConnectionString();
            logger.LogInformation("ConnectionString: {0}", connectionString);
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
        }
    }

}