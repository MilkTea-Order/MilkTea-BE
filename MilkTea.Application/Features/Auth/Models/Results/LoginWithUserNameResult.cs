using MilkTea.Application.Features.Auth.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class LoginWithUserNameResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public int UserId { get; set; } = default;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiresAt { get; set; } = default!;
        public List<PermissionDto> Permissions { get; set; } = default!;
    }
}

