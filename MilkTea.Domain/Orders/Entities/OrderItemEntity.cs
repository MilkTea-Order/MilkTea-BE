using MilkTea.Domain.Common.Abstractions;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.ValueObjects;

namespace MilkTea.Domain.Orders.Entities;

public sealed class OrderItemEntity : Entity<int>
{
    public int OrderId { get; private set; }

    public MenuItem MenuItem { get; private set; } = default!;

    public int Quantity { get; private set; }

    public OrderItemStatus Status { get; private set; } = OrderItemStatus.Pending;

    public decimal TotalAmount => MenuItem.Price * Quantity;

    public string? Note { get; private set; }

    public int? PerformBy { get; private set; }
    public DateTime? PerformDate { get; private set; }

    public int? CompletedBy { get; private set; }
    public DateTime? CompletedDate { get; private set; }

    public int? CancelledBy { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public string? CancelReason { get; private set; }
    public bool IsCancelled => CancelledBy.HasValue && CancelledDate.HasValue;

    private OrderItemEntity() { }

    internal static OrderItemEntity Create(MenuItem menuItem,
                                                int quantity,
                                                int createdBy,
                                                string? note = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentNullException.ThrowIfNull(menuItem);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);
        var now = DateTime.Now;
        return new OrderItemEntity
        {
            MenuItem = menuItem,
            Quantity = quantity,
            CreatedBy = createdBy,
            CreatedDate = now,
            Note = note
        };
    }

    /// <summary>
    /// Updates the quantity of the order item and records the user who made the update.
    /// </summary>
    /// <param name="quantity">The new quantity to set for the order item.</param>
    /// <param name="updatedBy">The identifier of the user performing the update.</param>
    /// <exception cref="OrderItemStatusInValidException">Thrown when the order item has current status != pending.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="quantity"/> or <paramref name="updatedBy"/> is less than or equal to zero.</exception>
    public void UpdateQuantity(int quantity, int updatedBy)
    {
        if (Status != OrderItemStatus.Pending) throw new OrderItemStatusInValidException();

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Quantity = quantity;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.Now;
    }

    /// <summary>
    /// Update note of itself
    /// </summary>
    /// <param name="note">note</param>
    /// <param name="updatedBy">who update</param>
    /// <exception cref="OrderItemStatusInValidException">Thrown when the order item has current status != pending.</exception>
    public void UpdateNote(string? note, int updatedBy)
    {
        if (Status != OrderItemStatus.Pending) throw new OrderItemStatusInValidException();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Note = note;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.Now;
    }

    public void UpdateOrderId(int orderId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderId);
        OrderId = orderId;
    }

    /// <summary>
    ///  Cancels the order item.
    /// </summary>
    /// <param name="cancelledBy">The identifier of the user performing the cancellation.</param>
    /// <exception cref="OrderItemStatusInValidException">Thrown when the order item has current status != pending.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="cancelledBy"/> is less than or equal to zero.</exception>"
    public void Cancel(int cancelledBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cancelledBy);
        if (Status != OrderItemStatus.Pending) throw new OrderItemStatusInValidException();

        Status = OrderItemStatus.Cancelled;
        CancelledBy = cancelledBy;
        CancelledDate = DateTime.Now;

        UpdatedBy = cancelledBy;
        UpdatedDate = CancelledDate;
    }

    /// <summary>
    /// Updates the status of the order item with sequential validation and audit fields.
    /// </summary>
    /// <param name="newStatus">The target status to transition to.</param>
    /// <param name="performedBy">The identifier of the user performing the update.</param>
    /// <param name="cancelReason">The reason for cancellation (required when transitioning to Cancelled status).</param>
    /// <exception cref="InvalidOrderDetailStatusTransitionException">Thrown when the status transition is not allowed.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="performedBy"/> is less than or equal to zero.</exception>
    public void UpdateStatus(OrderItemStatus newStatus, int performedBy, string? cancelReason = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(performedBy);

        if (!CanTransitionTo(newStatus))
            throw new InvalidOrderDetailStatusTransitionException(Status, newStatus);

        var now = DateTime.Now;
        Status = newStatus;
        UpdatedBy = performedBy;
        UpdatedDate = now;

        switch (newStatus)
        {
            case OrderItemStatus.InProgress:
                PerformBy = performedBy;
                PerformDate = now;
                break;
            case OrderItemStatus.Completed:
                CompletedBy = performedBy;
                CompletedDate = now;
                break;
            case OrderItemStatus.Cancelled:
                CancelledBy = performedBy;
                CancelledDate = now;
                CancelReason = cancelReason;
                break;
        }
    }

    private bool CanTransitionTo(OrderItemStatus target)
    {
        var allowed = Status switch
        {
            OrderItemStatus.Pending => new[] { OrderItemStatus.InProgress, OrderItemStatus.Cancelled },
            OrderItemStatus.InProgress => new[] { OrderItemStatus.Completed },
            _ => Array.Empty<OrderItemStatus>()
        };

        return allowed.Contains(target);
    }
}
