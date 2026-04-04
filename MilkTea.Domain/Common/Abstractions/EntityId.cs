namespace MilkTea.Domain.Common.Abstractions;

public class EntityId<TId> : IEntityId<TId>
{
    public TId Id { get; set; } = default!;
}

