using MilkTea.Application.Features.User.Model.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.User.Model.Results
{
    public class GetUserListResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public List<AccountProfile> Users { get; set; } = new();
    }
}
