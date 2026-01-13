using MilkTea.Shared.Extensions;
using System.Globalization;

namespace MilkTea.Shared.Utils
{
    public class DateHelper
    {
        public static bool IsValid(string? yyyyMMdd, string format = "yyyy-MM-dd")
        {
            if (yyyyMMdd.IsNullOrWhiteSpace())
                return false;

            return DateTime.TryParseExact(
                yyyyMMdd,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _
            );
        }

        public static DateTime? ToDateTime(string? s_date, string? format = null)
        {
            if (s_date.IsNullOrWhiteSpace())
            {
                return null;
            }
            try
            {
                if (!format.IsNullOrWhiteSpace())
                {
                    DateTime.TryParseExact(
                        s_date,
                        format!,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var ret
                    );
                    return ret;
                }

                var vDt = DateTime.Parse(s_date!, null, System.Globalization.DateTimeStyles.RoundtripKind);
                return new DateTime(
                    vDt.Year, vDt.Month, vDt.Day,
                    vDt.Hour, vDt.Minute, vDt.Second,
                    vDt.Kind // keep UTC/local kind
                );
            }
            catch (System.Exception)
            {
                return null;
            }

        }

    }
}
