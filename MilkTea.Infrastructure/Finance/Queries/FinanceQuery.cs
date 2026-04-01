using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Finance.Models.Dtos;
using MilkTea.Application.Ports.Time;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Finance.Queries
{
    public class FinanceQuery(AppDbContext Context, ITimeZoneServicePort timeZoneServicePort) : IFinanceQuery
    {
        private readonly AppDbContext _vContext = Context;

        public async Task<List<CollectionAndSpendGroupDto>> GetCollectionAndSpendGroupsAsync(CancellationToken cancellationToken = default)
        {
            return await _vContext.CollectAndSpendGroups.AsNoTracking()
                                              .Select(g => new CollectionAndSpendGroupDto
                                              {
                                                  Id = g.Id,
                                                  Name = g.Name
                                              })
                                              .ToListAsync(cancellationToken);
        }

        public async Task<List<FinanceTransactionDateDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);

            var rawData = await _vContext.CollectAndSpends
                .AsNoTracking()
                .Where(cs => cs.ActionDate >= fromDate && cs.ActionDate < toDate)
                .Join(
                    _vContext.CollectAndSpendGroups,
                    cs => cs.CollectAndSpendGroupID,
                    g => g.Id,
                    (cs, g) => new
                    {
                        cs.Id,
                        cs.Name,
                        cs.Amount,
                        cs.CreatedDate,
                        cs.ActionDate,
                        cs.Note,
                        GroupId = g.Id,
                        GroupName = g.Name,
                        Date = DateOnly.FromDateTime(cs.ActionDate.Date)
                    }
                )
                .ToListAsync(cancellationToken);

            var result = rawData
                .GroupBy(x => x.Date)
                .OrderByDescending(g => g.Key)
                .Select(dateGroup => new FinanceTransactionDateDto
                {
                    Date = dateGroup.Key,
                    TotalAmount = dateGroup.Sum(x => x.Amount),

                    Groups = dateGroup
                        .GroupBy(x => new { x.GroupId, x.GroupName })
                        .Select(group => new FinanceTranscationGroupDto
                        {
                            Id = group.Key.GroupId,
                            Name = group.Key.GroupName,
                            TotalAmount = group.Sum(x => x.Amount),

                            Items = group
                                .OrderByDescending(x => x.CreatedDate)
                                .Select(x => new CollectAndSpendItemDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Amount = x.Amount,
                                    ActionDate = x.ActionDate,
                                    CreatedDate = x.CreatedDate,
                                    Note = x.Note
                                })
                                .ToList()
                        })
                        .OrderBy(x => x.Id)
                        .ToList()
                })
                .ToList();

            return result;
        }
    }
}
