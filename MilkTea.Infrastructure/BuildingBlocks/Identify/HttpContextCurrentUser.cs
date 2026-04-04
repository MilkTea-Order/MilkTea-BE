using Microsoft.AspNetCore.Http;
using MilkTea.Application.Ports.Users;
using System.Security.Claims;

namespace MilkTea.Infrastructure.BuildingBlocks.Identify
{
    public sealed class HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor) : IIdentifyServicePorts
    {
        private readonly IHttpContextAccessor _vHttpContextAccessor = httpContextAccessor;

        public int UserId
        {
            get
            {
                var user = _vHttpContextAccessor.HttpContext?.User;
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

