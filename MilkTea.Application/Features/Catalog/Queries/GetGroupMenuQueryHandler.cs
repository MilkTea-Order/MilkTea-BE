using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuQueryHandler(
    ICatalogUnitOfWork catalogUnitOfWork) : IRequestHandler<GetGroupMenuQuery, GetGroupMenuResult>
{

    private readonly ICatalogUnitOfWork _vCatalogUnitOfWork = catalogUnitOfWork;
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuQuery query, CancellationToken cancellationToken)
    {
        var result = new GetGroupMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        CommonStatus? status = null;
        if (query.StatusId.HasValue && Enum.IsDefined(typeof(CommonStatus), query.StatusId.Value))
        {
            status = (CommonStatus)query.StatusId.Value;
        }

        MenuStatus? itemStatus = null;
        if (query.ItemStatusId.HasValue && Enum.IsDefined(typeof(MenuStatus), query.ItemStatusId.Value))
        {
            itemStatus = (MenuStatus)query.ItemStatusId.Value;
        }
        var groups = await _vCatalogUnitOfWork.Menus.GetByStatusWithMenuAsync(
                                                    query.StatusId.HasValue ? (int?)query.StatusId.Value : null,
                                                    query.ItemStatusId.HasValue ? (int?)query.ItemStatusId.Value : null,
                                                    cancellationToken);
        result.GroupMenu = groups.Select(g => new MenuGroupDto
        {
            MenuGroupId = g.Id,
            MenuGroupName = g.Name,
            StatusId = (int)g.Status,
            StatusName = g.Status.ToString(),
            Quantity = g.Menus.Count()
        }).ToList();
        return result;
    }
}
