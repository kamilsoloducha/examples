namespace EventBus.Configuration;

public record RabbitConfiguration
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public required  string UserName { get; init; }
    public required  string Password { get; init; }
    public required  string QueuePrefix { get; init; }
    public required  TimeSpan MaxDelay { get; init; }
}