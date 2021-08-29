using Blueprints.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShippingService.Database.Configuration;
using ShippingService.Model;

namespace ShippingService.Database
{
    public class ShippingDbContext : DbContext
    {

        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ILogger<ShippingDbContext> logger;

        public DbSet<Shipping> Shippings { get; set; }

        public ShippingDbContext(IConnectionStringProvider connectionStringProvider,
        ILogger<ShippingDbContext> logger)
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
            modelBuilder.ApplyConfiguration(new ShippingConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }

    }
}