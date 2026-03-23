using MilkTea.Application.Features.Users.Model.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Users.Model.Results
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public UserProfile? User { get; set; }
    }
}

