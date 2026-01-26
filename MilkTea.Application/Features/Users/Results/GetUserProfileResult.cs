using MilkTea.Application.DTOs.Users;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Users.Results
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public UserProfileDto? User { get; set; }
    }
}

