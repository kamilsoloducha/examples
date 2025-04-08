using MassTransit;

namespace Service3.Consumers.Definitions;

public class Service3From2Definition : ConsumerDefinition<Service3From2EventConsumer>
{
    public Service3From2Definition()
    {
    }
}