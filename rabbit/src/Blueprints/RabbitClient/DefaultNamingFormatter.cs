using Blueprints.RabbitClient.Abstraction;
using Blueprints.RabbitClient.Configuration;
using Microsoft.Extensions.Options;

namespace Blueprints.RabbitClient;

public class DefaultNamingFormatter : INamingFormatter
{
    private readonly RabbitConfiguration _configuration;

    public DefaultNamingFormatter(IOptions<RabbitConfiguration> options)
    {
        _configuration = options.Value;
    }
    
    public string QueueName(string eventName) => $"{_configuration.QueuePrefix}_{eventName}";
    public string ExchangeName(string eventName) =>$"{eventName}_exchange"; 
    public string ErrorQueueName(string eventName) => $"{QueueName(eventName)}_error";
}