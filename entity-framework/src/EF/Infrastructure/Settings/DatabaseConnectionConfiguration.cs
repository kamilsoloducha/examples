namespace EF.Infrastructure.Settigns;

public class DatabaseConnectionConfiguration
{
    public string Server { get; set; }
    public string Port { get; set; }
    public string Database { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}