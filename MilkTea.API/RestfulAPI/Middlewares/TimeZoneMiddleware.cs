using TimeZoneConverter;
namespace MilkTea.API.RestfulAPI.Middlewares
{
    public class TimeZoneMiddleware
    {
        private readonly RequestDelegate _next;

        public TimeZoneMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var tzHeader = context.Request.Headers["X-Timezone"].FirstOrDefault();

            TimeZoneInfo timeZone;

            if (!string.IsNullOrEmpty(tzHeader))
            {
                try
                {
                    timeZone = TZConvert.GetTimeZoneInfo(tzHeader);
                }
                catch
                {
                    // Không đúng định dạng hoặc không tồn tại thì mặc định UTC
                    timeZone = TimeZoneInfo.Utc;
                }
            }
            else
            {
                // Không truyền thì UTC
                timeZone = TimeZoneInfo.Utc;
            }

            context.Items["TimeZone"] = timeZone;

            await _next(context);
        }
    }
}
