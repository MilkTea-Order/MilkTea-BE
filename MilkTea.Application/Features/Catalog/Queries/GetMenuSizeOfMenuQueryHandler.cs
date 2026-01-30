using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuSizeOfMenuQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetMenuSizeOfMenuQuery, GetMenuSizeOfMenuResult>
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

        var menuGroup = await unitOfWork.Menus.GetByMenuIdWithRelationshipsAsync(query.MenuId, cancellationToken);

        var activePriceList = await unitOfWork.PriceLists.GetActivePriceListAsync();

        if (menuGroup is null || menuGroup.Status != CommonStatus.Active || activePriceList is null)
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
        var prices = await unitOfWork.PriceLists.GetPricesForMenuAsync(activePriceList.Id, query.MenuId, cancellationToken);
        var sizeDict = await unitOfWork.Sizes.GetByIdsAsync(menu.MenuSizes.Select(ms => ms.SizeID), cancellationToken);
        result.MenuSize = menu.MenuSizes.Select(ms =>
        {
            sizeDict.TryGetValue(ms.SizeID, out var size);
            return new MenuSizePriceDto
            {
                SizeId = ms.SizeID,
                SizeName = size?.Name ?? "Không rõ",
                RankIndex = size?.RankIndex ?? int.MaxValue,
                Price = prices.TryGetValue(ms.SizeID, out var price) ? price : 0m,
                CurrencyName = activePriceList.Currency?.Name,
                CurrencyCode = activePriceList.Currency?.Code,
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
