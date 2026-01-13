using MilkTea.Shared.Utils;

namespace MilkTea.Shared.Extensions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Checks whether the string is null, empty, or only whitespace.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Checks whether the string is null or empty (ignores whitespace).
        /// </summary>
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Converts null string to empty string (safe usage).
        /// </summary>
        public static string ToSafe(this string? value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Trims the string safely (null -> empty).
        /// </summary>
        public static string SafeTrim(this string? value)
        {
            return value?.Trim() ?? string.Empty;
        }


        public static bool IsValidDate(this string? value, string fmt = "dd/MM/yyyy")
        {
            return DateHelper.IsValid(value, fmt);
        }

        /// <summary>
        /// Convert to DateTime
        /// </summary>
        public static DateTime? TryToDateTime(this string? value, string fmt = "dd/MM/yyyy")
        {
            if (DateHelper.IsValid(value, fmt))
            {
                return (DateTime)DateHelper.ToDateTime(value, fmt)!;
            }
            return null;
        }

        /// <summary>
        /// Convert to DateTime
        /// </summary>
        public static DateTime ToDateTime(this string? value, string fmt = "dd/MM/yyyy")
        {
            return (DateTime)TryToDateTime(value, fmt)!;
        }

        /// <summary>
        /// Convert to DateTime
        /// </summary>
        public static DateTime? ToUtc(this string? value, string fmt = "dd/MM/yyyy")
        {
            if (DateHelper.IsValid(value, fmt))
            {
                var vParts = value!.Split("/");
                return new DateTime(int.Parse(vParts[2]), int.Parse(vParts[1]), int.Parse(vParts[0]), 0, 0, 0, DateTimeKind.Utc);
            }
            return null;
        }


        public static int? TryToInt(this string? value)
        {
            if (int.TryParse(value, out var intVal))
            {
                return intVal;
            }
            return null;
        }

        public static int ToInt(this string? value)
        {
            return (int)TryToInt(value)!;
        }
    }
}
