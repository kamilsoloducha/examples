using Microsoft.Extensions.Options;

namespace Blueprints.Database
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {

        private DatabaseConfig databaseConfig;

        public ConnectionStringProvider(IOptions<DatabaseConfig> options)
        {
            databaseConfig = options.Value;
        }

        public string GetConnectionString()
        => $"server={databaseConfig.Host}; port={databaseConfig.Port}; database={databaseConfig.Database}; user={databaseConfig.User}; password={databaseConfig.Password};Persist Security Info=False; Connect Timeout=300";
    }
}