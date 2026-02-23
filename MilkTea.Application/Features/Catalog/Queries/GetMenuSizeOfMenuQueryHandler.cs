using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuSizeOfMenuQuery : IRequest<GetMenuSizeOfMenuResult>
{
    public int MenuId { get; set; }
}

public sealed class GetMenuSizeOfMenuQueryHandler(
    ICatalogUnitOfWork catalogUnitOfWork) : IRequestHandler<GetMenuSizeOfMenuQuery, GetMenuSizeOfMenuResult>
{
    public async Task<GetMenuSizeOfMenuResult> Handle(GetMenuSizeOfMenuQuery query, CancellationToken cancellationToken)
    {
        var result = new GetMenuSizeOfMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        if (query.MenuId <= 0)
        {
            result.MenuSize = new List<MenuSizePriceDto>();
            return result;
        }

        var menuGroup = await catalogUnitOfWork.Menus.GetByMenuIdWithRelationshipsAsync(query.MenuId, cancellationToken);

        if (menuGroup is null || menuGroup.Status != CommonStatus.Active)
        {
            result.MenuSize = new List<MenuSizePriceDto>();
            return result;
        }

        var menu = menuGroup.Menus.SingleOrDefault();
        if (menu is null)
        {
            result.MenuSize = new List<MenuSizePriceDto>();
            return result;
        }
        var priceList = await catalogUnitOfWork.PriceLists.GetActiveByMenuWithRelationshipAsync(query.MenuId, cancellationToken);

        if (priceList is null)
        {
            result.MenuSize = new List<MenuSizePriceDto>();
            return result;
        }

        var sizeDict = await catalogUnitOfWork.Sizes.GetByIdsAsync(menu.MenuSizes.Select(ms => ms.SizeID), cancellationToken);
        result.MenuSize = menu.MenuSizes.Select(ms =>
        {
            sizeDict.TryGetValue(ms.SizeID, out var size);
            var priceListDetail = priceList.Details.FirstOrDefault(d => d.MenuID == query.MenuId && d.SizeID == ms.SizeID);
            return new MenuSizePriceDto
            {
                SizeId = ms.SizeID,
                SizeName = size?.Name ?? "Không rõ",
                RankIndex = size?.RankIndex ?? int.MaxValue,
                Price = priceListDetail?.Price ?? -1,
                PriceListId = priceList.Id,
                CurrencyId = priceList.Currency?.Id ?? 0,
                CurrencyName = priceList.Currency?.Name,
                CurrencyCode = priceList.Currency?.Code,
            };
        }).OrderBy(ms => ms.RankIndex).ToList();

        return result;
    }

    private static GetMenuSizeOfMenuResult SendError(GetMenuSizeOfMenuResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
