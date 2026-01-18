using MilkTea.Domain.Entities.Users;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Results.Users
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public User? User { get; set; }
    }
}
