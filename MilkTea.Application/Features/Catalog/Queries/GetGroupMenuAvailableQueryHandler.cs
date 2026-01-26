using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuAvailableQueryHandler(
    IMenuRepository menuRepository) : IRequestHandler<GetGroupMenuAvailableQuery, GetGroupMenuResult>
{
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuAvailableQuery query, CancellationToken cancellationToken)
    {
        var result = new GetGroupMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        var groups = await menuRepository.GetMenuGroupsAvailableAsync();
        var groupIds = groups.Select(g => g.Id).ToList();
        var quantityMap = await menuRepository.GetMenuCountsByGroupIdsAsync(groupIds);

        result.GroupMenu = groups.Select(g => new MenuGroupDto
        {
            MenuGroupId = g.Id,
            MenuGroupName = g.Name,
            StatusId = (int)g.Status,
            StatusName = g.Status.ToString(),
            Quantity = quantityMap.TryGetValue(g.Id, out var count) ? count : 0
        }).ToList();

        return result;
    }
}
