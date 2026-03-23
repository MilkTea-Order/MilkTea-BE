using MilkTea.Application.Features.Finance.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Finance.Models.Results
{
    public class GetTransactionGroupResult
    {
        public StringListEntry ResultData = new StringListEntry();
        public List<CollectionAndSpendGroupDto> Groups { get; set; } = new List<CollectionAndSpendGroupDto>();
    }
}
