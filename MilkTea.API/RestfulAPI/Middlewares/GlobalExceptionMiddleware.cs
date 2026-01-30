using MilkTea.API.RestfulAPI.Common;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.Exceptions;

namespace MilkTea.API.RestfulAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _vNext;
        private readonly ILogger<GlobalExceptionMiddleware> _vLogger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _vNext = next;
            _vLogger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _vNext(context);
            }
            catch (RequestValidationException vex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;

                var response = new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = vex.Message,
                    Description = $"{Parameters.CODE_SUCCESS} - {Parameters.TEXT_SUCCESS}, {Parameters.CODE_ERROR} - Error",
                    Data = vex.ErrorData
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _vLogger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ResultsReturned.Error("An unexpected error occurred. Please try again later.");

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
