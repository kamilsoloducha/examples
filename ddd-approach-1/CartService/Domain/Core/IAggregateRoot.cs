namespace Domain.Core;

public interface IAggregateRoot
{
    IEnumerable<IDomainEvent> DomainEvents { get; }
}