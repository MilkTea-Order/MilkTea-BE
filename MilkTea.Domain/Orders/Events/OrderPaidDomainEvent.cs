using MilkTea.Domain.Common.Abstractions;
using MilkTea.Domain.Orders.Entities;

namespace MilkTea.Domain.Orders.Events;

public sealed record OrderPaidDomainEvent(OrderEntity Order) : IDomainEvent;
