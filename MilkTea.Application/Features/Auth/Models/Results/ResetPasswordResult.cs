using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class ResetPasswordResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
