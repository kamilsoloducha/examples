using MassTransit.Topology;

namespace Blueprints.Rabbit
{
    public class ExchangeNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
        {
            var name = typeof(T).Name;
            return $"{name}_Exchange";
        }
    }
}
