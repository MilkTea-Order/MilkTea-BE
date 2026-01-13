using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Users
{
    public class UpdatePasswordResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
