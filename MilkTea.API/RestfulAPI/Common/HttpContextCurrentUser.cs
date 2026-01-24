using MilkTea.Application.Ports.Identity;
using System.Security.Claims;

namespace MilkTea.API.RestfulAPI.Common
{
    public sealed class HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    throw new UnauthorizedAccessException("User is not authenticated.");

                var raw = user.FindFirstValue("UserId") ?? user.FindFirstValue("UserID");
                if (string.IsNullOrEmpty(raw) || !int.TryParse(raw, out var userId) || userId <= 0)
                    throw new UnauthorizedAccessException("Invalid user id claim.");

                return userId;
            }
        }
    }
}

