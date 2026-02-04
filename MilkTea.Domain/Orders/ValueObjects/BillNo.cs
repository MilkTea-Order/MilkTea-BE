namespace MilkTea.Domain.Orders.ValueObjects;

public sealed record BillNo(string Value)
{
    public static BillNo Create(string prefix, DateTime date, int createdBy, int sequence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);
        if (sequence < 1) throw new ArgumentOutOfRangeException(nameof(sequence));

        var value = $"{prefix}{date:yyyyMMdd}{createdBy}{sequence}";
        return new BillNo(value);
    }

    public static BillNo FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("BillNo cannot be empty.", nameof(value));

        return new BillNo(value);
    }

    public override string ToString() => Value;
}
