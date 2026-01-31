using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Shared.Domain.Constants;
namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuItemsAvailableOfGroupQueryHandler(
    ICatalogUnitOfWork catalogUnitOfWork) : IRequestHandler<GetMenuItemsAvailableOfGroupQuery, GetMenuItemsOfGroupResult>
{
    public async Task<GetMenuItemsOfGroupResult> Handle(GetMenuItemsAvailableOfGroupQuery query, CancellationToken cancellationToken)
    {
        var result = new GetMenuItemsOfGroupResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        if (query.GroupId <= 0)
            return SendError(result, ErrorCode.E0036, "GroupID");

        var menus = await catalogUnitOfWork.Menus.GetByIdWithMenuAsync(query.GroupId, (int)MenuStatus.Active, cancellationToken);

        if (menus == null)
        {
            result.Menus = new List<MenuItemDto>();
        }
        else
        {
            result.Menus = menus.Menus.Select(m => new MenuItemDto
            {
                MenuId = m.Id,
                MenuCode = m.Code,
                MenuName = m.Name,
                MenuGroupId = m.MenuGroupID,
                MenuGroupName = menus.Name,
                StatusId = (int)m.Status,
                StatusName = m.Status.ToString()
            }).ToList();
        }
        return result;
    }

    private static GetMenuItemsOfGroupResult SendError(GetMenuItemsOfGroupResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
