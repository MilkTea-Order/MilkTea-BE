using MilkTea.Application.Features.Finance.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Finance.Models.Results
{
    public class GetSummaryCollectAndSpendResult
    {
        public StringListEntry ResultData { get; set; } = new StringListEntry();

        public List<CollectAndSpendGroupDto> Summary { get; set; } = new List<CollectAndSpendGroupDto>();
    }
}
