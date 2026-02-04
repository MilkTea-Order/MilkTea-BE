using MilkTea.Domain.SharedKernel.Exceptions;

namespace MilkTea.Domain.Orders.Exceptions
{
    public sealed class OrderCancelledException : DomainException
    {
        public OrderCancelledException()
            : base("Order has been cancelled.") { }
    }

    public sealed class OrderNotEditableException : DomainException
    {
        public OrderNotEditableException()
            : base("Order must unpaid status.") { }
    }

    public sealed class OrderItemCancelledException : DomainException
    {
        public OrderItemCancelledException()
            : base("Order item has been cancelled.") { }
    }

    public sealed class OrderItemNotFoundException : DomainException
    {
        public OrderItemNotFoundException()
            : base("Order item not found.") { }
    }
}
