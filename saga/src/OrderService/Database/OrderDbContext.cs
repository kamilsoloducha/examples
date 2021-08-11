using Blueprints.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Database.Configuration;
using OrderService.Model;

namespace OrderService.Database
{
    public class OrderDbContext : DbContext
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        private readonly ILogger<OrderDbContext> logger;

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        public OrderDbContext(IConnectionStringProvider connectionStringProvider,
        ILogger<OrderDbContext> logger)
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
            modelBuilder.ApplyConfiguration<Order>(new OrderConfiguration());
            modelBuilder.ApplyConfiguration<Item>(new ItemConfiguration());
        }
    }
}