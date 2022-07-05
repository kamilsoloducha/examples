namespace Company.Sagas
{
    using MassTransit;

    public class MassTransitSagaSagaDefinition :
        SagaDefinition<MassTransitSagaSaga>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<MassTransitSagaSaga> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}