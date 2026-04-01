using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Results;
using MilkTea.Shared.Domain.Constants;
namespace MilkTea.Application.Features.Catalog.Queries;


public sealed class GetMenuItemsAvailableOfGroupQuery : IRequest<GetMenuItemsOfGroupResult>
{
    public int GroupId { get; set; }
}

public sealed class GetMenuItemsAvailableOfGroupQueryHandler(
    ICatalogQuery catalogQuery) : IRequestHandler<GetMenuItemsAvailableOfGroupQuery, GetMenuItemsOfGroupResult>
{
    private readonly ICatalogQuery _vCatalogQuery = catalogQuery;
    public async Task<GetMenuItemsOfGroupResult> Handle(GetMenuItemsAvailableOfGroupQuery query, CancellationToken cancellationToken)
    {
        var result = new GetMenuItemsOfGroupResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.Now);
        if (query.GroupId <= 0)
        {
            result.Menus = new List<MenuDto>();
            return result;
        }
        var menus = await _vCatalogQuery.GetMenusAsync(query.GroupId, null, cancellationToken);
        result.Menus = menus;
        return result;
    }

    private static GetMenuItemsOfGroupResult SendError(GetMenuItemsOfGroupResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
