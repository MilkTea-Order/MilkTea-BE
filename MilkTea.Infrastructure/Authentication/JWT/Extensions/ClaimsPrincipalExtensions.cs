using System.Security.Claims;

namespace MilkTea.Infrastructure.Authentication.JWT.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("UserId")?.Value;
        }
    }
}
