using MilkTea.Application.Features.Users.Model.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Users.Model.Results
{
    public class GetUserListResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<UserProfile> Users { get; set; } = new();
    }
}
