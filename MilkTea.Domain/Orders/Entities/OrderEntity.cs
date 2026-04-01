using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Events;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Entities;

public sealed class OrderEntity : Aggregate<int>
{
    private readonly List<OrderItemEntity> _vOrderItems = new();
    public IReadOnlyList<OrderItemEntity> OrderItems => _vOrderItems.AsReadOnly();

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

    public static OrderEntity Create(
        int dinnerTableId,
        int orderBy,
        int createdBy,
        string? note = null)
    {
        var now = DateTime.Now;
        var order = new OrderEntity
        {
            DinnerTableId = dinnerTableId,
            OrderBy = orderBy,
            OrderDate = now,
            CreatedBy = createdBy,
            CreatedDate = now,
            Status = OrderStatus.Unpaid,
            Note = note
        };

        //if (!string.IsNullOrWhiteSpace(note))
        //{
        //    order.AddNoteBy = createdBy;
        //    order.AddNoteDate = now;
        //}
        return order;
    }

    public void CreateOrderItem(MenuItem menuItem, int quantity, int createdBy, string? note = null)
    {
        EnsureCanEdit();
        var orderItem = OrderItemEntity.Create(menuItem, quantity, createdBy, note);
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
        if (_vOrderItems.All(x => x.IsCancelled)) Cancel(removedBy);
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
        UpdatedDate = DateTime.Now;
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
        UpdatedDate = DateTime.Now;
    }


    /// <summary>
    /// Cancels the order, recording the user who performed the cancellation.
    /// </summary>
    /// <param name="cancelledBy">The identifier of the user performing the cancellation.</param>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    public void Cancel(int cancelledBy)
    {
        EnsureCanEdit();
        UpdateStatus(OrderStatus.Cancelled, cancelledBy);
        CancelledBy = cancelledBy;
        CancelledDate = DateTime.Now;
        //foreach (var item in _vOrderItems.Where(x => !x.IsCancelled))
        //{
        //    item.Cancel(cancelledBy);
        //}
        //Touch(cancelledBy);
    }


    public void UpdateNote(string note, int updatedBy)
    {
        Note = note;
        AddNoteBy = updatedBy;
        AddNoteDate = DateTime.Now;
        Touch(updatedBy);
    }

    /// <summary>
    /// Updates the dinner table identifier and records the user and timestamp of the change.
    /// </summary>
    /// <param name="dinnerTableId">The identifier of the dinner table to assign.</param>
    /// <param name="updatedBy">The identifier of the user making the change.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="dinnerTableId"/> or <paramref name="updatedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order is not in an editable state.</exception>
    public void ChangTable(int dinnerTableId, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dinnerTableId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);
        EnsureCanEdit();
        ChangeBy = updatedBy;
        ChangeDate = DateTime.Now;
        DinnerTableId = dinnerTableId;
        //Touch(updatedBy);
    }
    /// <summary>
    /// Merges order items from a source order into the current order and records the merge operation.
    /// The source order must be in an unpaid status to be eligible for merging.
    /// </summary>
    /// <param name="orderSource">The source order whose items will be merged into the current order.</param>
    /// <param name="mergedBy">The identifier of the user performing the merge operation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="mergedBy"/> is less than or equal to zero.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the current order is not in an editable state.</exception>
    /// <exception cref="OrderSourceCannotMergeException">Thrown when the source order status is not Unpaid and cannot be merged.</exception>
    public void MergeFrom(OrderEntity orderSource, int mergedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mergedBy);
        EnsureCanEdit();
        if (orderSource.Status != OrderStatus.Unpaid) throw new OrderSourceCannotMergeException();
        MergedBy = mergedBy;
        MergedDate = DateTime.Now;
        // Update orderID
        foreach (var item in orderSource.OrderItems)
        {
            item.UpdateOrderId(Id);
        }
    }

    /// <summary>
    /// Marks the order as paid, assigns bill number, updates payment details, applies promotion discount, and sets
    /// order status to NotCollected.
    /// </summary>
    /// <param name="prefix">The prefix to use when assigning the bill number.</param>
    /// <param name="payBy">The identifier of the user making the payment.</param>
    /// <param name="paymentType">The type of payment used.</param>
    /// <exception cref="NotExistBillPrefix">Thrown when the bill prefix does not exist.</exception>
    /// <exception cref="NotExistActionBy">Thrown when the action by does not exist.</exception>
    /// <exception cref="NotExistPaymentType">Thrown when the payment type does not exist.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order cannot be edited because its status is not Unpaid.</exception>
    public void Payment(string prefix, int payBy, string paymentType)
    {
        EnsureCanEdit();

        if (payBy <= 0) throw new NotExistActionBy();
        if (string.IsNullOrWhiteSpace(paymentType)) throw new NotExistPaymentType();

        var now = DateTime.Now;
        AssignBillNo(prefix, now);

        var totalAmount = GetTotalAmountForPay();
        var discount = GetPromotionDiscount();

        PaymentedBy = payBy;
        PaymentedDate = now;
        PaymentedTotal = totalAmount - discount;
        PaymentedType = paymentType;

        if (Promotion is not null) Promotion.AssignAmount(discount);

        TotalAmount = totalAmount;

        Status = OrderStatus.NotCollected;
        //AddDomainEvent(new OrderPaidDomainEvent(this));
    }

    /// <summary>
    /// Marks the order as collected and updates its status to Paid.
    /// </summary>
    /// <param name="collectedBy">Identifier of the user who collected the order.</param>
    /// <exception cref="NotExistActionBy">Thrown when the collectedBy identifier is invalid.</exception>
    /// <exception cref="OrderNotEditableException">Thrown when the order cannot be edited due to its current status.</exception>
    public void MaskAsCollected(int collectedBy)
    {
        if (collectedBy <= 0) throw new NotExistActionBy();
        if (this.Status != OrderStatus.NotCollected)
        {
            throw new OrderNotEditableException();
        }
        Status = OrderStatus.Paid;
        ActionBy = collectedBy;
        ActionDate = DateTime.Now;
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
    private decimal GetTotalAmountForPay()
    {
        var subtotal = _vOrderItems.Where(x => !x.IsCancelled).Sum(x => x.TotalAmount);
        return subtotal;
    }

    private decimal GetPromotionDiscount()
    {
        if (Promotion is null) return 0m;
        var subtotal = GetTotalAmountForPay();
        var discount = Promotion.CalculateDiscount(subtotal);
        return discount;
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.Now;
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
        {
            throw new NotExistOrderStatus();
        }
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

    private void AssignBillNo(string? prefix, DateTime time)
    {
        EnsureCanEdit();
        if (BillNo is null) BillNo = BillNo.Create(prefix!, time);
    }


    public void FinalizeAndPublishCreated()
    {
        AddDomainEvent(new OrderCreatedDomainEvent(this));
    }

    public void FinalizeAndPublishCollected()
    {
        AddDomainEvent(new OrderCollectedDomainEvent(Id,
                            OrderItems
                            .Where(x => !x.IsCancelled)
                            .Select(x => new OrderCollectedItem(x.MenuItem.MenuId, x.MenuItem.SizeId, x.Quantity)).ToList()
                        ));
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
