using MassTransit;

namespace Blueprints.RabbitMassTransit;

public class ExchangeNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        var name = typeof(T).Name;
        return $"{name}_Exchange";
    }
}