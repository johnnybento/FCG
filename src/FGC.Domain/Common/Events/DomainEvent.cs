
using FGC.Core.Common.Events.Interfaces;

namespace FGC.Core.Common.Events;

/// <summary>
/// Base para eventos de domínio, já popula OccurredOn.
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
