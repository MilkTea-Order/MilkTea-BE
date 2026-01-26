using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.Orders.Events;
using MilkTea.Domain.Orders.ValueObjects;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Orders.Entities;

public sealed class Order : Aggregate<int>
{
    private readonly List<OrderItem> _vOrderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _vOrderItems.AsReadOnly();

    public BillNo BillNo { get; set; } = default!;

    public int DinnerTableId { get; set; }

    public OrderStatus Status { get; set; }

    public int OrderBy { get; set; }
    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; private set; }

    public string? Note { get; set; }

    public int? PaymentedBy { get; set; }
    public DateTime? PaymentedDate { get; set; }
    public decimal? PaymentedTotal { get; set; }
    public string? PaymentedType { get; set; }

    public int? CancelledBy { get; set; }
    public DateTime? CancelledDate { get; set; }

    public Promotion? Promotion { get; private set; }

    public int? AddNoteBy { get; set; }
    public DateTime? AddNoteDate { get; set; }
    public int? ChangeBy { get; set; }
    public DateTime? ChangeDate { get; set; }
    public int? MergedBy { get; set; }
    public DateTime? MergedDate { get; set; }
    public int? ActionBy { get; set; }
    public DateTime? ActionDate { get; set; }

    public static Order Create(
        BillNo billNo,
        int dinnerTableId,
        int orderBy,
        int createdBy,
        string? note = null)
    {
        var now = DateTime.UtcNow;

        var order = new Order
        {
            BillNo = billNo,
            DinnerTableId = dinnerTableId,
            OrderBy = orderBy,
            OrderDate = now,
            CreatedBy = createdBy,
            CreatedDate = now,
            Status = OrderStatus.Unpaid,
            Note = note,
            TotalAmount = 0m
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
        EnsureNotCancelled();

        var orderItem = OrderItem.Create(menuItem, quantity, createdBy, note);
        _vOrderItems.Add(orderItem);

        RecalculateTotalAmount();
        Touch(updatedBy: createdBy);
    }

    public void RemoveOrderItem(int orderItemId, int removedBy)
    {
        EnsureNotCancelled();

        var item = _vOrderItems.FirstOrDefault(x => x.Id == orderItemId);
        if (item is null) return;

        if (!item.IsCancelled)
        {
            item.Cancel(removedBy);
            RecalculateTotalAmount();
            Touch(updatedBy: removedBy);
        }
    }

    public void FinalizeAndPublishCreated()
    {
        AddDomainEvent(new OrderCreatedDomainEvent(this));
    }

    public void UpdateStatus(OrderStatus newStatus, int changedBy)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            throw new ArgumentException("Invalid order status.", nameof(newStatus));

        Status = newStatus;
        Touch(changedBy);
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

        RecalculateTotalAmount();

        AddDomainEvent(new OrderCancelledDomainEvent(this, cancelledBy));
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
        EnsureNotCancelled();

        Promotion = promotion;
        RecalculateTotalAmount();
        Touch(updatedBy: appliedBy);
    }

    public void RemovePromotion(int updatedBy)
    {
        EnsureNotCancelled();

        Promotion = null;
        RecalculateTotalAmount();
        Touch(updatedBy);
    }

    public void UpdateNote(string note, int updatedBy)
    {
        Note = note;
        AddNoteBy = updatedBy;
        AddNoteDate = DateTime.UtcNow;
        Touch(updatedBy);
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

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }

    private void EnsureNotCancelled()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is cancelled.");
    }
}
