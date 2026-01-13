using MilkTea.API.RestfulAPI.Common;

namespace MilkTea.API.RestfulAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 1. Log lỗi (BẮT BUỘC)
                _logger.LogError(ex, "Unhandled exception");

                // 2. Trả response chuẩn
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ResultsReturned.Error("An unexpected error occurred. Please try again later.");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
