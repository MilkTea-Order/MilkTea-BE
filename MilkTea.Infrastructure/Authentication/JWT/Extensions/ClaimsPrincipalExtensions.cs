using System.Security.Claims;

namespace MilkTea.Infrastructure.Authentication.JWT.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            // Backward compatible claim keys
            return principal.FindFirst("UserId")?.Value
                ?? principal.FindFirst("UserID")?.Value;
        }
    }
}
