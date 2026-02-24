using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Entities;

/// <summary>
/// Order item entity (child of Order aggregate).
/// Maps to ordersdetail table.
/// </summary>
public sealed class OrderItemEntity : Entity<int>
{
    public int OrderId { get; private set; }
    internal OrderEntity? Order { get; private set; }

    public MenuItem MenuItem { get; private set; } = default!;

    public int Quantity { get; private set; }

    public decimal TotalAmount => MenuItem.Price * Quantity;

    public string? Note { get; private set; }

    // Cancellation
    public int? CancelledBy { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public bool IsCancelled => CancelledBy.HasValue;

    // For EF
    private OrderItemEntity() { }

    internal static OrderItemEntity Create(
        MenuItem menuItem,
        int quantity,
        int createdBy,
        string? note = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentNullException.ThrowIfNull(menuItem);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

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
    /// <exception cref="OrderItemCancelledException">Thrown if the order item has been cancelled.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="quantity"/> or <paramref name="updatedBy"/> is less than or equal to zero.</exception>
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

    /// <summary>
    ///  Cancels the order item.
    /// </summary>
    /// <param name="cancelledBy">The identifier of the user performing the cancellation.</param>
    /// <exception cref="OrderItemCancelledException">Thrown when the order item has already been cancelled.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="cancelledBy"/> is less than or equal to zero.</exception>"
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
