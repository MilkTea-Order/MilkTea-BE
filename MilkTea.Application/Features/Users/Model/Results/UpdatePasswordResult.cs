using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Users.Model.Results
{
    public class UpdatePasswordResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}

