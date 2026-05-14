namespace MilkTea.Shared.Extensions;

public static class EnumExtensions
{
    public static bool TryParseEnum<TEnum>(this string? value,
                                            out TEnum result,
                                            bool ignoreCase = true)
                                            where TEnum : struct, Enum
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value))
            return false;
        return Enum.TryParse(value, ignoreCase, out result);
    }
}