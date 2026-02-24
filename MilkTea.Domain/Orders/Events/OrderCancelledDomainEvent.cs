using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Events;

public sealed record OrderCancelledDomainEvent(OrderEntity Order, int CancelledBy) : IDomainEvent;
