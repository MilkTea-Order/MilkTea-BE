using MilkTea.Application.Features.User.Model.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.User.Model.Results
{
    public class GetUserProfileResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public AccountProfile? User { get; set; }
    }
}

