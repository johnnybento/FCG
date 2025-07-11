using FGC.Core.Common.Entities.Interfaces;

namespace FGC.Core.Common.Entities;

public abstract class EntityBase<TId> : IEntity<TId>
{
    public TId Id { get; protected set; }

    protected EntityBase() { }

    protected EntityBase(TId id)
    {
        Id = id;
    }

    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(object @event)
        => _domainEvents.Add(@event);

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    public override bool Equals(object obj)
    {
        if (obj is not EntityBase<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }
    public override int GetHashCode() => HashCode.Combine(Id);
}