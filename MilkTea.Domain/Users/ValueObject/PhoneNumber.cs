using MilkTea.Domain.SharedKernel.Constants;
using System.Text.RegularExpressions;

namespace MilkTea.Domain.Users.ValueObject;

public sealed record PhoneNumber(string Value)
{
    private static readonly Regex PhoneRegex =
        new(@"^\+?[0-9]{8,15}$", RegexOptions.Compiled);

    public static PhoneNumber Empty() => new(string.Empty);

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public static PhoneNumber Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(ErrorCode.E0036, "cellphone");

        value = value.Trim()
                     .Replace(" ", "")
                     .Replace("-", "");

        if (!PhoneRegex.IsMatch(value))
            throw new ArgumentException(ErrorCode.E0036, "cellphone");

        return new PhoneNumber(value);
    }

    public static PhoneNumber OfOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? Empty() : Of(value);

    public override string ToString() => Value;
}
