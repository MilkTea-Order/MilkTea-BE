namespace MilkTea.Domain.Orders.ValueObjects;

/// <summary>
/// Value Object representing a promotion applied to an order.
/// This is a snapshot of promotion at the time of application.
/// Nullable because promotion is optional and time-dependent.
/// </summary>
public sealed class Promotion : IEquatable<Promotion>
{
    public int PromotionId { get; }
    public int? Percent { get; }
    public decimal? Amount { get; }

    private Promotion(int promotionId, int? percent, decimal? amount)
    {
        PromotionId = promotionId;
        Percent = percent;
        Amount = amount;
    }

    /// <summary>
    /// Creates a percentage-based promotion.
    /// </summary>
    public static Promotion CreatePercent(int promotionId, int percent)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(promotionId);
        if (percent <= 0 || percent > 100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be between 1 and 100.");

        return new Promotion(promotionId, percent, null);
    }

    /// <summary>
    /// Creates an amount-based promotion.
    /// </summary>
    public static Promotion CreateAmount(int promotionId, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(promotionId);
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");

        return new Promotion(promotionId, null, amount);
    }

    /// <summary>
    /// Calculates the discount amount based on the order total.
    /// </summary>
    public decimal CalculateDiscount(decimal orderTotal)
    {
        if (Percent.HasValue)
        {
            return orderTotal * Percent.Value / 100;
        }

        if (Amount.HasValue)
        {
            return Math.Min(Amount.Value, orderTotal);
        }

        return 0;
    }

    /// <summary>
    /// Gets the final amount after applying promotion discount.
    /// </summary>
    public decimal ApplyTo(decimal orderTotal)
    {
        var discount = CalculateDiscount(orderTotal);
        return Math.Max(0, orderTotal - discount);
    }

    // Value Object equality
    public bool Equals(Promotion? other)
    {
        if (other is null) return false;
        return PromotionId == other.PromotionId &&
               Percent == other.Percent &&
               Amount == other.Amount;
    }

    public override bool Equals(object? obj) => Equals(obj as Promotion);

    public override int GetHashCode() => HashCode.Combine(PromotionId, Percent, Amount);

    public static bool operator ==(Promotion? left, Promotion? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Promotion? left, Promotion? right) => !(left == right);
}
