using Blueprints.RabbitMassTransit;

namespace Service1.Services;

public class Service1Identificator : IServiceIdentificator
{
    public string Id => "Service1";
}