using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuQueryHandler(
    IMenuRepository menuRepository) : IRequestHandler<GetGroupMenuQuery, GetGroupMenuResult>
{
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuQuery query, CancellationToken cancellationToken)
    {
        var result = new GetGroupMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        // Convert StatusId to enum if provided
        CommonStatus? status = null;
        if (query.StatusId.HasValue && Enum.IsDefined(typeof(CommonStatus), query.StatusId.Value))
        {
            status = (CommonStatus)query.StatusId.Value;
        }

        CommonStatus? itemStatus = null;
        if (query.ItemStatusId.HasValue && Enum.IsDefined(typeof(CommonStatus), query.ItemStatusId.Value))
        {
            itemStatus = (CommonStatus)query.ItemStatusId.Value;
        }

        var groups = await menuRepository.GetMenuGroupsByStatusAsync(
            query.StatusId.HasValue ? (int?)query.StatusId.Value : null,
            query.ItemStatusId.HasValue ? (int?)query.ItemStatusId.Value : null);
        result.GroupMenu = groups.Select(g => new MenuGroupDto
        {
            MenuGroupId = g.Id,
            MenuGroupName = g.Name,
            StatusId = (int)g.Status,
            StatusName = g.Status.ToString(),
            Quantity = 0 // Will be calculated if needed
        }).ToList();

        return result;
    }
}
