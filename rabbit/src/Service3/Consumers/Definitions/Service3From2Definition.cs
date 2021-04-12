using MassTransit.Definition;
using Service3.Consumers;

namespace Service3.Consumers.Definitions
{
    public class Service3From2Definition : ConsumerDefinition<Service3From2EventConsumer>
    {
        public Service3From2Definition()
        {
        }
    }
}
