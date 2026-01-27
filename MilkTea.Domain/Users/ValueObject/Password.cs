namespace MilkTea.Domain.Users.ValueObject
{
    public sealed record Password(string value)
    {
        public static Password Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Password required", nameof(value));
            }
            return new Password(value);
        }
    }
}
