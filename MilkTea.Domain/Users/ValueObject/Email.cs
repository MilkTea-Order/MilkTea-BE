using MilkTea.Domain.SharedKernel.Constants;
using System.Text.RegularExpressions;

namespace MilkTea.Domain.Users.ValueObject;

public sealed record Email(string Value)
{
    private static readonly Regex EmailRegex =
        new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

    public static Email Empty() => new(string.Empty);

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public static Email Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(ErrorCode.E0036, "email");

        value = value.Trim();

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException(ErrorCode.E0036, "email");

        return new Email(value);
    }

    public static Email OfOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? Empty() : Of(value);

    public override string ToString() => Value;
}
