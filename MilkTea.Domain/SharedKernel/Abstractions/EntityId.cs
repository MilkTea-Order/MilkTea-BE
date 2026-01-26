namespace MilkTea.Domain.SharedKernel.Abstractions
{
    public class EntityId<TId> : IEntityId<TId>
    {
        public TId Id { get; set; } = default!;
    }
}
