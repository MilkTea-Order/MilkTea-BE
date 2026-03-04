using MilkTea.Domain.Orders.Exceptions;

namespace MilkTea.Domain.Orders.ValueObjects;

public sealed record BillNo(string Value)
{
    public static BillNo Create(string prefix, DateTime time)
    {
        if (string.IsNullOrWhiteSpace(prefix)) throw new NotExistBillPrefix();
        var value = $"{prefix}{time:yyyyMMddHHmmss}";
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
