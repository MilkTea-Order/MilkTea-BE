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
        private readonly ITimeZoneServicePort _vTimeZoneServicePort = timeZoneServicePort;
        public async Task<List<CollectAndSpendGroupDto>> GetSummaryAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        {
            var tz = _vTimeZoneServicePort.GetTimeZone();
            var rawData = await _vContext.CollectAndSpends.AsNoTracking()
                                                           .Where(cs => cs.CreatedDate >= fromDate && cs.CreatedDate <= toDate)
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
                                                                   GroupId = g.Id,
                                                                   GroupName = g.Name,
                                                                   Date = DateOnly.FromDateTime(
                                                                                TimeZoneInfo.ConvertTimeFromUtc(cs.CreatedDate, tz)
                                                                            )
                                                               }
                                                           )
                                                           .OrderByDescending(x => x.CreatedDate)
                                                           .ToListAsync(cancellationToken);

            var result = rawData.GroupBy(x => new { x.GroupId, x.GroupName })
                                    .Select(group => new CollectAndSpendGroupDto
                                    {
                                        GroupId = group.Key.GroupId,
                                        GroupName = group.Key.GroupName,
                                        TotalAmount = group.Sum(x => x.Amount),
                                        Dates = group.GroupBy(x => x.Date)
                                                        .OrderByDescending(g => g.Key)
                                                        .Select(dateGroup => new CollectAndSpendDateDto
                                                        {
                                                            Date = dateGroup.Key,
                                                            TotalAmount = dateGroup.Sum(x => x.Amount),
                                                            Items = dateGroup.OrderByDescending(x => x.CreatedDate)
                                                                                .Select(x => new CollectAndSpendItemDto
                                                                                {
                                                                                    Id = x.Id,
                                                                                    Name = x.Name,
                                                                                    Amount = x.Amount,
                                                                                    CreatedDate = x.CreatedDate
                                                                                })
                                                                                .ToList()
                                                        })
                                                        .ToList()
                                    })
                                    .OrderBy(x => x.GroupId)
                                    .ToList();
            return result;
        }
    }
}
