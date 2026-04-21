using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class CreateOtpResult
    {
        public StringListEntry ResultData { get; set; } = new StringListEntry();
        public int SessionId { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
