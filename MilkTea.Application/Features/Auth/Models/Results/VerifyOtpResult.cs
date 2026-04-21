using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class VerifyOtpResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public bool IsVerified { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiresAt { get; set; }
    }
}
