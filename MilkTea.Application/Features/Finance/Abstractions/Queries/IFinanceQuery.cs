using MilkTea.Application.Features.Finance.Models.Dtos;

namespace MilkTea.Application.Features.Finance.Abstractions.Queries
{
    public interface IFinanceQuery
    {
        Task<List<CollectAndSpendGroupDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    }
}
