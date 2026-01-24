using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Users;

namespace MilkTea.Application.Results.Users
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public UserProfileDto? User { get; set; }
    }
}
