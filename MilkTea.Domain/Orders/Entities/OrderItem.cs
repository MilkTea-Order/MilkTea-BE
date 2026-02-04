using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Entities;

/// <summary>
/// Order item entity (child of Order aggregate).
/// Maps to ordersdetail table.
/// </summary>
public sealed class OrderItem : Entity<int>
{
    public int OrderId { get; private set; }
    internal Order? Order { get; private set; }

    public MenuItem MenuItem { get; private set; } = default!;

    public int Quantity { get; private set; }

    public decimal TotalAmount => MenuItem.Price * Quantity;

    public string? Note { get; private set; }

    // Cancellation
    public int? CancelledBy { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public bool IsCancelled => CancelledBy.HasValue;

    // For EF
    private OrderItem() { }

    internal static OrderItem Create(
        MenuItem menuItem,
        int quantity,
        int createdBy,
        string? note = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentNullException.ThrowIfNull(menuItem);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new OrderItem
        {
            MenuItem = menuItem,
            Quantity = quantity,
            CreatedBy = createdBy,
            CreatedDate = now,
            Note = note
        };
    }

    public void UpdateQuantity(int quantity, int updatedBy)
    {
        if (IsCancelled) throw new OrderItemCancelledException();

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Quantity = quantity;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }

    public void UpdateNote(string? note, int updatedBy)
    {
        if (IsCancelled) throw new OrderItemCancelledException();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Note = note;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }

    public void Cancel(int cancelledBy)
    {
        if (IsCancelled) throw new OrderItemCancelledException();

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cancelledBy);

        CancelledBy = cancelledBy;
        CancelledDate = DateTime.UtcNow;

        UpdatedBy = cancelledBy;
        UpdatedDate = CancelledDate;
    }
}
