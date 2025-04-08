using MassTransit;

namespace Service2.Consumers.Definitions;

public class Service2Definition : ConsumerDefinition<Service2EventConsumer>
{
    public Service2Definition()
    {
    }
}