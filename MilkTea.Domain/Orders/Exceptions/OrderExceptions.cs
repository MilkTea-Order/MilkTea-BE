using MilkTea.Domain.Common.Exceptions;
using MilkTea.Domain.Orders.Enums;

namespace MilkTea.Domain.Orders.Exceptions
{
    public sealed class OrderCancelledException() : DomainException("Order has been cancelled.");

    public sealed class OrderNotEditableException() : DomainException("Order must unpaid status.");

    public sealed class OrderItemStatusInValidException() : DomainException("Order item status is invalid.");

    public sealed class OrderItemCancelledException() : DomainException("Order item has been cancelled.");

    public sealed class OrderItemNotFoundException() : DomainException("Order item not found.");

    public sealed class OrderSourceCannotMergeException() : DomainException("Order cannot be merged.");

    public sealed class NotExistOrderStatus() : DomainException("Order status is not exist.");

    public sealed class NotExistBillPrefix() : DomainException("Bill prefix does not exist.");

    public sealed class NotExistPaymentType() : DomainException("Payment type does not exist.");

    public sealed class NotExistActionBy() : DomainException("Action by does not exist.");

    public sealed class InvalidOrderDetailStatusTransitionException(OrderItemStatus from, OrderItemStatus to)
        : DomainException($"Cannot transition order item status from '{from}' to '{to}'.");
}
