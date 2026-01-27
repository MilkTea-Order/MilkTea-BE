using System.Globalization;

namespace MilkTea.Domain.Users.ValueObject;

public sealed record BirthDay(string Value)
{
    public const string Format = "dd/MM/yyyy";
    public const int MinAge = 10;
    public const int MaxAge = 100;

    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    public static BirthDay Empty() => new(string.Empty);

    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public static BirthDay Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("BirthDay is required.", nameof(value));

        value = value.Trim();

        if (!DateTime.TryParseExact(value, Format, Culture, DateTimeStyles.None, out var birthDate))
            throw new ArgumentException($"BirthDay must be a valid date in format {Format}.", nameof(value));

        var today = DateTime.UtcNow.Date;

        if (birthDate.Date > today)
            throw new ArgumentException("BirthDay cannot be in the future.", nameof(value));

        var age = CalculateAge(birthDate, today);

        if (age < MinAge || age > MaxAge)
            throw new ArgumentException($"Age must be between {MinAge} and {MaxAge}.", nameof(value));

        return new BirthDay(birthDate.ToString(Format, Culture));
    }

    public static BirthDay OfOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? Empty() : Of(value);

    public DateOnly AsDateOnly()
    {
        if (IsEmpty)
            throw new InvalidOperationException("BirthDay is empty.");

        var date = DateTime.ParseExact(Value, Format, Culture);
        return DateOnly.FromDateTime(date);
    }

    private static int CalculateAge(DateTime birthDate, DateTime today)
    {
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    public override string ToString() => Value;
}
