using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Users
{
    public class RefreshAccessTokenResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}

