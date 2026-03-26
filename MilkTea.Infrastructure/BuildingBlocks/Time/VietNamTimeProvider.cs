using MilkTea.Application.Ports.Time;

namespace MilkTea.Infrastructure.BuildingBlocks.Time
{
    public class VietNamTimeProvider : ITimeServicePort
    {
        private readonly TimeZoneInfo _tz;

        public VietNamTimeProvider()
        {
            try
            {
                _tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
            }
            catch (TimeZoneNotFoundException)
            {
                _tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"); // Linux/macOS
            }
        }

        public DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _tz);

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime ToLocal(DateTime utc)
        {
            utc = utc.Kind == DateTimeKind.Utc
                ? utc
                : DateTime.SpecifyKind(utc, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utc, _tz);
        }

        public DateTime ToUtc(DateTime local)
        {
            local = local.Kind == DateTimeKind.Unspecified
                ? local
                : DateTime.SpecifyKind(local, DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(local, _tz);
        }
    }
}