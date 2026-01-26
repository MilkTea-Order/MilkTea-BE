using MilkTea.Domain.Pricing.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Pricing.Entities;

/// <summary>
/// Promotion on total bill entity (Aggregate Root).
/// </summary>
public sealed class PromotionOnTotalBill : Aggregate<int>
{
    public string Name { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime StopDate { get; private set; }
    public int? ProPercent { get; private set; }
    public decimal? ProAmount { get; private set; }
    
    /// <summary>
    /// Promotion status. Maps to StatusID column.
    /// </summary>
    public PromotionStatus Status { get; private set; }
    
    public string? Note { get; private set; }

    // For EF Core
    private PromotionOnTotalBill() { }

    public static PromotionOnTotalBill CreatePercent(
        string name,
        DateTime startDate,
        DateTime stopDate,
        int percent,
        int createdBy,
        string? note = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (stopDate <= startDate)
            throw new ArgumentException("StopDate must be after StartDate.");

        if (percent <= 0 || percent > 100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be between 1 and 100.");

        var now = DateTime.UtcNow;

        return new PromotionOnTotalBill
        {
            Name = name,
            StartDate = startDate,
            StopDate = stopDate,
            ProPercent = percent,
            ProAmount = null,
            Status = PromotionStatus.Draft,
            Note = note,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public static PromotionOnTotalBill CreateAmount(
        string name,
        DateTime startDate,
        DateTime stopDate,
        decimal amount,
        int createdBy,
        string? note = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (stopDate <= startDate)
            throw new ArgumentException("StopDate must be after StartDate.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        var now = DateTime.UtcNow;

        return new PromotionOnTotalBill
        {
            Name = name,
            StartDate = startDate,
            StopDate = stopDate,
            ProPercent = null,
            ProAmount = amount,
            Status = PromotionStatus.Draft,
            Note = note,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive() => Status == PromotionStatus.Active && 
                              DateTime.UtcNow >= StartDate && 
                              DateTime.UtcNow <= StopDate;

    public void Activate(int activatedBy)
    {
        if (Status == PromotionStatus.Active)
            throw new InvalidOperationException("Promotion is already active.");

        if (DateTime.UtcNow < StartDate)
            throw new InvalidOperationException("Cannot activate promotion before start date.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = PromotionStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == PromotionStatus.Draft)
            throw new InvalidOperationException("Promotion is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = PromotionStatus.Draft;
        Touch(deactivatedBy);
    }

    public void Expire(int expiredBy)
    {
        if (Status == PromotionStatus.Expired)
            throw new InvalidOperationException("Promotion is already expired.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expiredBy);

        Status = PromotionStatus.Expired;
        Touch(expiredBy);
    }

    public void Cancel(int cancelledBy)
    {
        if (Status == PromotionStatus.Cancelled)
            throw new InvalidOperationException("Promotion is already cancelled.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cancelledBy);

        Status = PromotionStatus.Cancelled;
        Touch(cancelledBy);
    }

    public decimal CalculateDiscount(decimal totalAmount)
    {
        if (!IsActive()) return 0;
        
        if (ProPercent.HasValue && ProPercent.Value > 0)
        {
            return totalAmount * ProPercent.Value / 100;
        }
        
        if (ProAmount.HasValue && ProAmount.Value > 0)
        {
            return Math.Min(ProAmount.Value, totalAmount);
        }
        
        return 0;
    }

    public void UpdateInfo(string name, string? note, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Note = note;
        Touch(updatedBy);
    }

    public void UpdateDates(DateTime startDate, DateTime stopDate, int updatedBy)
    {
        if (stopDate <= startDate)
            throw new ArgumentException("StopDate must be after StartDate.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        StartDate = startDate;
        StopDate = stopDate;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
