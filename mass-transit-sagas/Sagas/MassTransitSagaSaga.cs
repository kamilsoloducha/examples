namespace Company.Sagas
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class MassTransitSagaSaga :
        ISaga,
        InitiatedBy<InitiateMassTransitSaga>,
        Orchestrates<UpdateMassTransitSaga>,
        Observes<NotifyMassTransitSaga, MassTransitSagaSaga>
    {
        public Guid CorrelationId { get; set; }
        public string Value { get; set; }

        public Expression<Func<MassTransitSagaSaga, NotifyMassTransitSaga, bool>> CorrelationExpression =>
            (saga, message) => saga.Value == message.Value;

        public Task Consume(ConsumeContext<InitiateMassTransitSaga> context)
        {
            Value = context.Message.Value;

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<UpdateMassTransitSaga> context)
        {
            Value = context.Message.Value;
            
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<NotifyMassTransitSaga> context)
        {
            return Task.CompletedTask;
        }
    }
}

