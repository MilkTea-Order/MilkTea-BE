namespace MilkTea.Domain.SharedKernel.Abstractions;


public interface IAggregate<TId> : IAggregate, IEntityId<TId> { }


public interface IAggregate : IAuditable
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    IDomainEvent[] ClearDomainEvents();
}
