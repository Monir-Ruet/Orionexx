using System.ComponentModel.DataAnnotations.Schema;

namespace Orionexx.Core.Shared.Primitives;

public abstract class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = [];
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RemoveDomainEvent(BaseEvent domainEvent) =>_domainEvents.Remove(domainEvent);
}


public abstract class BaseEntity<T> : BaseEntity, IEquatable<BaseEntity<T>>
{
    public T Id { get; init; } = default!;

    public bool Equals(BaseEntity<T>? other)
    {
        if (other is null || other.GetType() != GetType())
            return false;

        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType() || obj is not BaseEntity<T> entity)
            return false;

        return EqualityComparer<T>.Default.Equals(Id, entity.Id);
    }

    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Id!);

    public static bool operator ==(BaseEntity<T>? first, BaseEntity<T>? second)
    {
        if (first is null && second is null)
            return true;
        if (first is null || second is null)
            return false;

        return first.Equals(second);
    }

    public static bool operator !=(BaseEntity<T>? first, BaseEntity<T>? second) => !(first ==second);
}
