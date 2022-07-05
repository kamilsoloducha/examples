namespace Contracts
{
    using System;
    using MassTransit;

    public record UpdateMassTransitSaga :
        CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}