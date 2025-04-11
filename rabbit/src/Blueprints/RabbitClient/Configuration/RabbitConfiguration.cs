namespace Blueprints.RabbitClient.Configuration;

public class RabbitConfiguration
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string QueuePrefix { get; init; }
}