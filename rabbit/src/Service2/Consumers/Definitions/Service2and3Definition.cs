using MassTransit.Definition;

namespace Service2.Consumers.Definitions
{
    public class Service2and3Definition : ConsumerDefinition<Service2and3EventConsumer>
    {
        public Service2and3Definition()
        {
        }
    }
}
