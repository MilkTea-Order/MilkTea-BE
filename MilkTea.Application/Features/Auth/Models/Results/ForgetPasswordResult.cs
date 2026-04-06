using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class ForgetPasswordResult
    {
        public StringListEntry ResultData { get; set; } = new StringListEntry();
        public DateTime? ExpiresAt { get; set; }
    }
}
