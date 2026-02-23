using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions;
using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries
{
    public sealed class GetMenuItemAvailableByGroupAndMenuNameQuery : IRequest<GetMenuItemsOfGroupResult>
    {
        public int? GroupId { get; set; }
        public string? MenuName { get; set; } = null;
    }
    public class GetMenuItemAvailableByGroupAndMenuNameQueryHandler(ICatalogQuery catalogQuery) : IRequestHandler<GetMenuItemAvailableByGroupAndMenuNameQuery, GetMenuItemsOfGroupResult>
    {
        private readonly ICatalogQuery _vCatalogQuery = catalogQuery;
        public async Task<GetMenuItemsOfGroupResult> Handle(GetMenuItemAvailableByGroupAndMenuNameQuery query, CancellationToken cancellationToken)
        {
            var result = new GetMenuItemsOfGroupResult();
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            if (query.GroupId <= 0)
            {
                result.Menus = new List<MenuDto>();
                return result;
            }

            var menus = await _vCatalogQuery.GetMenusAsync(query.GroupId, query.MenuName, cancellationToken);
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
}
