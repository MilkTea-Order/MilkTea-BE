using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Events;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Entities;

public sealed class Order : Aggregate<int>
{
    private readonly List<OrderItem> _vOrderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _vOrderItems.AsReadOnly();

    public int DinnerTableId { get; private set; }
    public int OrderBy { get; private set; }
    public DateTime OrderDate { get; private set; }
    public int? CancelledBy { get; private set; }
    public DateTime? CancelledDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? Note { get; private set; }

    public int? PaymentedBy { get; private set; }
    public DateTime? PaymentedDate { get; private set; }
    public decimal? PaymentedTotal { get; private set; }
    public string? PaymentedType { get; private set; }

    public int? AddNoteBy { get; private set; }
    public DateTime? AddNoteDate { get; private set; }
    public int? ChangeBy { get; private set; }
    public DateTime? ChangeDate { get; private set; }
    public int? MergedBy { get; private set; }
    public DateTime? MergedDate { get; private set; }

    public BillNo? BillNo { get; private set; }
    public Promotion? Promotion { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public int? ActionBy { get; private set; }
    public DateTime? ActionDate { get; private set; }

    public static Order Create(
        //BillNo billNo,
        int dinnerTableId,
        int orderBy,
        int createdBy,
        string? note = null)
    {
        var now = DateTime.UtcNow;

        var order = new Order
        {
            //BillNo = billNo,
            DinnerTableId = dinnerTableId,
            OrderBy = orderBy,
            OrderDate = now,
            CreatedBy = createdBy,
            CreatedDate = now,
            Status = OrderStatus.Unpaid,
            Note = note
            //TotalAmount = 0m
        };

        if (!string.IsNullOrWhiteSpace(note))
        {
            order.AddNoteBy = createdBy;
            order.AddNoteDate = now;
        }

        return order;
    }

    public void CreateOrderItem(MenuItem menuItem, int quantity, int createdBy, string? note = null)
    {
        EnsureCanEdit();
        var orderItem = OrderItem.Create(menuItem, quantity, createdBy, note);
        _vOrderItems.Add(orderItem);
        Touch(updatedBy: createdBy);
    }

    /// <summary>
    /// Cancels the specified order item and records the user who performed the cancellation.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the order item to cancel.</param>
    /// <param name="removedBy">The identifier of the user performing the cancellation.</param>
    /// <exception cref="OrderItemNotFoundException">Thrown when the specified order item does not exist.</exception>
    /// <exception cref="OrderItemCancelledException">Thrown when the order item has already been cancelled.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="removedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>"
    public void CancelOrderItem(int orderItemId, int removedBy)
    {
        EnsureCanEdit();
        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) throw new OrderItemNotFoundException();
        item.Cancel(removedBy);
        Touch(removedBy);
    }


    /// <summary>
    /// Cancels the specified order items and records the user who performed the cancellation.
    /// </summary>
    /// <param name="orderItemIds">A list of order item IDs to cancel.</param>
    /// <param name="removedBy">The ID of the user performing the cancellation.</param>
    /// <exception cref="OrderItemNotFoundException">Thrown if any of the specified order items cannot be found.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="removedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    /// <exception cref="OrderItemCancelledException">Thrown when any of the order items have already been cancelled.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="orderItemIds"/> is null.</exception>
    public void CancelOrderItems(List<int> orderItemIds, int removedBy)
    {
        EnsureCanEdit();
        ArgumentNullException.ThrowIfNull(orderItemIds);
        foreach (var item in orderItemIds)
        {
            var orderItem = _vOrderItems.FirstOrDefault(x => x.Id == item);
            if (orderItem is null) throw new OrderItemNotFoundException();
            orderItem.Cancel(removedBy);
        }
        Touch(removedBy);
    }

    /// <summary>
    /// Updates the quantity of a specific order item and records the user who made the change.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the order item to update.</param>
    /// <param name="quantity">The new quantity to set for the order item.</param>
    /// <param name="updatedBy">The identifier of the user performing the update.</param>
    /// <exception cref="OrderItemNotFoundException">Thrown when the specified order item does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="orderItemId"/>, <paramref name="quantity"/>, or <paramref name="updatedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    /// <exception cref="OrderItemCancelledException">Thrown when the order item has already been cancelled.</exception>
    public void UpdateItemQuantity(int orderItemId, int quantity, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderItemId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) throw new OrderItemNotFoundException();

        EnsureCanEdit();

        item.UpdateQuantity(quantity, updatedBy);

        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the note for a specific order item and records the user and time of the update.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the order item to update.</param>
    /// <param name="note">The new note to associate with the order item.</param>
    /// <param name="updatedBy">The identifier of the user performing the update.</param>
    /// <exception cref="OrderItemNotFoundException">Thrown when the specified order item does not exist.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="orderItemId"/> or <paramref name="updatedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    /// <exception cref="OrderItemCancelledException">Thrown when the order item has already been cancelled.</exception>
    public void UpdateItemNote(int orderItemId, string? note, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderItemId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) throw new OrderItemNotFoundException();
        EnsureCanEdit();
        item.UpdateNote(note, updatedBy);
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }


    /// <summary>
    /// Cancels the order and all associated items, recording the user who performed the cancellation.
    /// </summary>
    /// <param name="cancelledBy">The identifier of the user performing the cancellation.</param>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    public void Cancel(int cancelledBy)
    {
        EnsureCanEdit();
        UpdateStatus(OrderStatus.Cancelled, cancelledBy);
        CancelledBy = cancelledBy;
        CancelledDate = DateTime.UtcNow;
        foreach (var item in _vOrderItems.Where(x => !x.IsCancelled))
        {
            item.Cancel(cancelledBy);
        }
        Touch(cancelledBy);
    }


    public void UpdateNote(string note, int updatedBy)
    {
        Note = note;
        AddNoteBy = updatedBy;
        AddNoteDate = DateTime.UtcNow;
        Touch(updatedBy);
    }

    public void AssignTable(int dinnerTableId, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dinnerTableId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        DinnerTableId = dinnerTableId;
        Touch(updatedBy);
    }

    public void FinalizeAndPublishCreated()
    {
        AddDomainEvent(new OrderCreatedDomainEvent(this));
    }


    public void AssignBillNo(
    string prefix,
    int sequence,
    int assignedBy,
    DateTime? date = null)
    {
        EnsureCanEdit();

        if (BillNo is not null) throw new InvalidOperationException("BillNo already assigned.");

        var billDate = date ?? DateTime.UtcNow;

        BillNo = BillNo.Create(prefix, billDate, assignedBy, sequence);

        Touch(assignedBy);
    }

    public void MarkAsPaid(int paidBy, decimal total, string paymentType)
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Order is already paid.");

        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total), "Payment total cannot be negative.");

        ArgumentException.ThrowIfNullOrWhiteSpace(paymentType);

        Status = OrderStatus.Paid;
        PaymentedBy = paidBy;
        PaymentedDate = DateTime.UtcNow;
        PaymentedTotal = total;
        PaymentedType = paymentType;

        Touch(paidBy);

        AddDomainEvent(new OrderPaidDomainEvent(this));
    }

    public void ApplyPromotion(Promotion promotion, int appliedBy)
    {
        ArgumentNullException.ThrowIfNull(promotion);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(appliedBy);
        EnsureCanEdit();

        Promotion = promotion;
        //RecalculateTotalAmount();
        Touch(updatedBy: appliedBy);
    }

    public void RemovePromotion(int updatedBy)
    {
        EnsureCanEdit();

        Promotion = null;
        //RecalculateTotalAmount();
        Touch(updatedBy);
    }


    /// <summary>
    /// Get Total Amount before payment (no excluded cancelled items)
    /// </summary>
    /// <returns></returns>
    public decimal GetTotalAmount()
    {
        var subtotal = _vOrderItems.Where(x => !x.IsCancelled).Sum(x => x.TotalAmount);
        if (Promotion is not null)
        {
            var discount = Promotion.CalculateDiscount(subtotal);
            subtotal = Math.Max(0, subtotal - discount);
        }
        return subtotal;
    }

    /// <summary>
    /// Get Total Amount for Pay (excluding cancelled items)
    /// </summary>
    /// <returns></returns>
    public decimal GetTotalAmountForPay()
    {
        var subtotal = _vOrderItems.Where(x => !x.IsCancelled).Sum(x => x.TotalAmount);
        if (Promotion is not null)
        {
            var discount = Promotion.CalculateDiscount(subtotal);
            subtotal = Math.Max(0, subtotal - discount);
        }
        return subtotal;
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }


    /// <summary>
    /// Updates the order status to the specified value.
    /// </summary>
    /// <param name="newStatus">The new status to set for the order.</param>
    /// <param name="changedBy">The identifier of the user making the change.</param>
    /// <exception cref="NotExistOrderStatus">Thrown when the specified order status does not exist.</exception>
    private void UpdateStatus(OrderStatus newStatus, int changedBy)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            throw new NotExistOrderStatus();
        Status = newStatus;
    }
    private void EnsureNotCancelled()
    {
        if (Status == OrderStatus.Cancelled)
            throw new OrderCancelledException();
    }

    /// <summary>
    /// Throws an exception if the order status is not Unpaid.
    /// </summary>
    /// <exception cref="OrderNotEditableException">Thrown when the order cannot be edited because its status is not Unpaid.</exception>
    private void EnsureCanEdit()
    {
        if (Status != OrderStatus.Unpaid) throw new OrderNotEditableException();
    }


    private void RecalculateTotalAmount()
    {
        var subtotal = _vOrderItems.Where(x => !x.IsCancelled).Sum(x => x.TotalAmount);

        if (Promotion is not null)
        {
            var discount = Promotion.CalculateDiscount(subtotal);
            TotalAmount = Math.Max(0, subtotal - discount);
        }
        else
        {
            TotalAmount = subtotal;
        }
    }



}
