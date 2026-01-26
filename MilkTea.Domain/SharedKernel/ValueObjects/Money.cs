namespace MilkTea.Domain.SharedKernel.ValueObjects
{
    public readonly record struct Money(decimal Value)
    {
        public static Money Of(decimal value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
            // DB bạn đang decimal(10,0) => không cho lẻ:
            if (value != decimal.Truncate(value))
                throw new ArgumentException("Money must be a whole number (no decimals) for this schema.", nameof(value));

            return new Money(value);
        }

        public static Money Zero => new(0);

        public static Money operator +(Money a, Money b) => new(a.Value + b.Value);
        public static Money operator -(Money a, Money b)
        {
            var v = a.Value - b.Value;
            if (v < 0) throw new InvalidOperationException("Money cannot be negative.");
            return new Money(v);
        }
    }
}
