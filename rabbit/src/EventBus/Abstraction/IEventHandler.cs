using System.Diagnostics.CodeAnalysis;

namespace EventBus.Abstraction;

public interface IEventHandler<in TEvent>
{
    Task Handle([NotNull] TEvent @event, CancellationToken cancellationToken = default);
}