using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Results;
using MilkTea.Domain.Catalog;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Common.Enums;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;


public sealed class GetMenuItemsOfGroupQuery : IRequest<GetMenuItemsOfGroupResult>
{
    public int GroupId { get; init; }
    public bool IsMenuActive { get; init; } = true;
}
public sealed class GetMenuItemsOfGroupQueryHandler(ICatalogQuery catalogQuery) 
                                                    : IRequestHandler<GetMenuItemsOfGroupQuery, GetMenuItemsOfGroupResult>
{
    private readonly ICatalogQuery _vCatalogQuery = catalogQuery;
    public async Task<GetMenuItemsOfGroupResult> Handle(GetMenuItemsOfGroupQuery query, CancellationToken cancellationToken)
    {
        var result = new GetMenuItemsOfGroupResult();

        if (query.GroupId <= 0)
        {
            return SendError(result, ErrorCode.E0036, "GroupID");
        }

        // Convert MenuStatusId to enum if provided (default active menu)
        var menuStatus = query.IsMenuActive ? CommonStatus.Active : CommonStatus.Inactive;

        var menus = await _vCatalogQuery.GetMenusAsync(query.GroupId, 
                                                                        null, 
                                                                        menuStatus, 
                                                                        cancellationToken: cancellationToken);

        result.Menus = menus.Count == 0 ? new List<MenuDto>() : menus;
        return result;
    }

    private static GetMenuItemsOfGroupResult SendError(GetMenuItemsOfGroupResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
