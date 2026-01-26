namespace MilkTea.Domain.Orders.ValueObjects;

/// <summary>
/// Value object for order bill number.
/// Format: {Prefix}{yyyyMMdd}{createdBy}{sequence}
/// </summary>
public sealed class BillNo : IEquatable<BillNo>
{
    public string Value { get; }

    private BillNo(string value) => Value = value;

    public static BillNo Create(string prefix, DateTime date, int createdBy, int sequence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);
        if (sequence < 1) throw new ArgumentOutOfRangeException(nameof(sequence));

        var value = $"{prefix}{date:yyyyMMdd}{createdBy}{sequence}";
        return new BillNo(value);
    }

    /// <summary>
    /// For EF Core mapping from existing string column.
    /// </summary>
    public static BillNo FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("BillNo cannot be empty.", nameof(value));
        return new BillNo(value);
    }

    public override string ToString() => Value;

    // Value Object equality
    public bool Equals(BillNo? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as BillNo);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(BillNo? left, BillNo? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(BillNo? left, BillNo? right) => !(left == right);

    // Implicit conversion for convenience
    public static implicit operator string(BillNo billNo) => billNo.Value;
}
