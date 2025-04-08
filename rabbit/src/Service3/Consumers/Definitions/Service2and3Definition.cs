using MassTransit;

namespace Service3.Consumers.Definitions;

public class Service2and3Definition : ConsumerDefinition<Service2and3EventConsumer>
{
    public Service2and3Definition()
    {
    }
}