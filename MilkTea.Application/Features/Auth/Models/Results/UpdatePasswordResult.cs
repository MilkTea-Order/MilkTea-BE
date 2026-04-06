using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Auth.Models.Results
{
    public class UpdatePasswordResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}

