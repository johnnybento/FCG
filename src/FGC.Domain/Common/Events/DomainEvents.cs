
using FGC.Core.Common.Events.Interfaces;

namespace FGC.Core.Common.Events;

/// <summary>
/// Coleta e expõe eventos disparados pelo domínio.
/// </summary>
public static class DomainEvents
{
    [ThreadStatic]
    private static List<IDomainEvent> _events;

    public static IReadOnlyCollection<IDomainEvent> Events
        => _events != null
           ? _events.AsReadOnly()
           : Array.Empty<IDomainEvent>();

    public static void Raise(IDomainEvent domainEvent)
    {
        _events ??= new List<IDomainEvent>();
        _events.Add(domainEvent);
    }

    public static void Clear() => _events?.Clear();
}