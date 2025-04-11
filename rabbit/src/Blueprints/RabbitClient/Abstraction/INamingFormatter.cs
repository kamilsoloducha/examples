namespace Blueprints.RabbitClient.Abstraction;

public interface INamingFormatter
{
    string QueueName(string eventName);
    string ExchangeName(string eventName);
    string ErrorQueueName(string eventName);
}