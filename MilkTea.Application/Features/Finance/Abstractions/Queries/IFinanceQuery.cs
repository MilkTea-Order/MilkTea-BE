using MilkTea.Application.Features.Finance.Models.Dtos;

namespace MilkTea.Application.Features.Finance.Abstractions.Queries
{
    public interface IFinanceQuery
    {
        Task<List<FinanceTranscationGroupDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

        Task<List<CollectionAndSpendGroupDto>> GetCollectionAndSpendGroupsAsync(CancellationToken cancellationToken = default);
    }
}
