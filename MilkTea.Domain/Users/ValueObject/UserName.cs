namespace MilkTea.Domain.Users.ValueObject
{
    public sealed record UserName(string value)
    {
        public static UserName Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Username required", nameof(value));
            }
            value = value.Trim();
            if (value.Length < 3 || value.Length > 50)
            {
                throw new ArgumentException("Username must be between 3 and 50 characters", nameof(value));
            }
            return new UserName(value);
        }

        public override string ToString() => value;
    }
}
