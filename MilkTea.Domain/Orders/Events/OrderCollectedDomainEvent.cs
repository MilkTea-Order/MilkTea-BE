using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Orders.Events
{
    public record OrderCollectedDomainEvent(int OrderId, IReadOnlyList<OrderCollectedItem> Items) : IDomainEvent;

    public record OrderCollectedItem(int MenuId, int SizeId, decimal Quantity);
}
