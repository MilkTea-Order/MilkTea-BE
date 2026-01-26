namespace MilkTea.Domain.SharedKernel.Abstractions;

/// <summary>
/// Base class for aggregate roots.
/// </summary>
public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _vDomainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _vDomainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _vDomainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var copy = _vDomainEvents.ToArray();
        _vDomainEvents.Clear();
        return copy;
    }
}
