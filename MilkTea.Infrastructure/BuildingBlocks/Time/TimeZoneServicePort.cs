using Microsoft.AspNetCore.Http;
using MilkTea.Application.Ports.Time;

namespace MilkTea.Infrastructure.BuildingBlocks.Time
{
    public class TimeZoneServicePort(IHttpContextAccessor httpContextAccessor) : ITimeZoneServicePort
    {
        private readonly IHttpContextAccessor _vHttpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Retrieves the current HTTP context's TimeZoneInfo, or returns UTC if not set.
        /// </summary>
        /// <returns>The TimeZoneInfo associated with the current HTTP context, or TimeZoneInfo.Utc if unavailable.</returns>
        public TimeZoneInfo GetTimeZone()
        {
            return _vHttpContextAccessor.HttpContext?.Items["TimeZone"] as TimeZoneInfo ?? TimeZoneInfo.Utc;
        }
    }
}
