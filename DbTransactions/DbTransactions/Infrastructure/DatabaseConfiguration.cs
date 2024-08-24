namespace DbTransactions.Infrastructure;

public class DatabaseConfiguration
{
    public DatabaseType DatabaseType { get; set; }
    public string ConnectionString { get; set; }
}

public enum DatabaseType
{
    Postgres,
    Mysql,
    Sqlserver
} 
