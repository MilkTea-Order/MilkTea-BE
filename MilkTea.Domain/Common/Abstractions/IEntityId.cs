namespace MilkTea.Domain.Common.Abstractions;

public interface IEntityId<TId>
{
    TId Id { get; set; }
}
