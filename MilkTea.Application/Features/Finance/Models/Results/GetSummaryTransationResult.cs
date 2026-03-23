using MilkTea.Application.Features.Finance.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Finance.Models.Results
{
    public class GetSummaryTransationResult
    {
        public StringListEntry ResultData { get; set; } = new StringListEntry();

        public List<FinanceTranscationGroupDto> Summary { get; set; } = new List<FinanceTranscationGroupDto>();
    }
}
