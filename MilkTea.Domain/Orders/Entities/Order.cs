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

        //RecalculateTotalAmount();
        Touch(updatedBy: createdBy);
    }

    public void CancelOrderItem(int orderItemId, int removedBy)
    {
        EnsureCanEdit();

        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null)
            throw new InvalidOperationException($"OrderItem with ID {orderItemId} not found in this order.");

        if (item.IsCancelled)
            throw new InvalidOperationException($"OrderItem with ID {orderItemId} is already cancelled.");

        item.Cancel(removedBy);
        //RecalculateTotalAmount();
        Touch(updatedBy: removedBy);
    }
    public List<int> CancelOrderItems(List<int> orderItemIds, int removedBy)
    {
        EnsureCanEdit();
        ArgumentNullException.ThrowIfNull(orderItemIds);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(removedBy);
        var cancelledIDs = new List<int>();
        if (orderItemIds.Count == 0)
            return new List<int>();

        var itemsToRemove = _vOrderItems
            .Where(x => orderItemIds.Contains(x.Id) && !x.IsCancelled)
            .ToList();

        if (itemsToRemove.Count == 0)
            return new List<int>();

        foreach (var item in itemsToRemove)
        {
            item.Cancel(removedBy);
            cancelledIDs.Add(item.Id);
        }
        Touch(removedBy);
        return cancelledIDs;
    }

    public void UpdateItemQuantity(int orderItemId, int quantity, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderItemId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) throw new OrderItemNotFoundException();

        EnsureCanEdit();

        item.UpdateQuantity(quantity, updatedBy);

        //RecalculateTotalAmount();

        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }

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


    public void Cancel(int cancelledBy)
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled.");

        UpdateStatus(OrderStatus.Cancelled, cancelledBy);

        CancelledBy = cancelledBy;
        CancelledDate = DateTime.UtcNow;

        foreach (var item in _vOrderItems.Where(x => !x.IsCancelled))
            item.Cancel(cancelledBy);

        //RecalculateTotalAmount();

        AddDomainEvent(new OrderCancelledDomainEvent(this, cancelledBy));
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

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }


    private void UpdateStatus(OrderStatus newStatus, int changedBy)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            throw new ArgumentException("Invalid order status.", nameof(newStatus));

        Status = newStatus;
        Touch(changedBy);
    }
    private void EnsureNotCancelled()
    {
        if (Status == OrderStatus.Cancelled)
            throw new OrderCancelledException();
    }
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
