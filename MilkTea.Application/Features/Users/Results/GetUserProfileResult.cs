using MilkTea.Application.Models.Users;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Users.Results
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public UserProfile? User { get; set; }
    }
}

