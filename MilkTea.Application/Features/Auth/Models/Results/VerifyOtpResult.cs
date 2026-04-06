using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class VerifyOtpResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
