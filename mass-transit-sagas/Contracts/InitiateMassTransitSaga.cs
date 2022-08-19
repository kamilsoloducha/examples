namespace Contracts
{
    using System;
    using MassTransit;

    public record InitiateMassTransitSaga :
        CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}