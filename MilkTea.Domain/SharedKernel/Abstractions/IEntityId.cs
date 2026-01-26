namespace MilkTea.Domain.SharedKernel.Abstractions;

public interface IEntityId<TId>
{
    TId Id { get; set; }
}
