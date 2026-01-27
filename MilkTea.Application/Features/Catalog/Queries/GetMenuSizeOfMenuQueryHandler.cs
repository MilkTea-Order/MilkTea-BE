using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Domain.SharedKernel.Constants;
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
            return SendError(result, ErrorCode.E0036, "MenuID");

        // Check menu is available
        var menu = await unitOfWork.Menus.GetMenuByIDAndAvaliableAsync(query.MenuId);
        if (menu is null)
            return SendError(result, ErrorCode.E0040, "MenuID");

        var activePriceList = await unitOfWork.PriceLists.GetActivePriceListAsync();
        if (activePriceList is null)
        {
            result.MenuSize = new List<MenuSizePriceDto>();
            return result;
        }

        // Get menu sizes available
        var menuSizes = await unitOfWork.Menus.GetMenuSizesAvailableByMenuAsync(query.MenuId);
        var prices = await unitOfWork.PriceLists.GetPricesForMenuAsync(activePriceList.Id, query.MenuId);

        result.MenuSize = menuSizes.Select(ms => new MenuSizePriceDto
        {
            SizeId = ms.SizeID,
            SizeName = ms.Size?.Name,
            RankIndex = ms.Size?.RankIndex ?? 0,
            Price = prices.TryGetValue(ms.SizeID, out var price) ? price : 0m,
            CurrencyName = activePriceList.Currency?.Name,
            CurrencyCode = activePriceList.Currency?.Code
        }).ToList();

        return result;
    }

    private static GetMenuSizeOfMenuResult SendError(GetMenuSizeOfMenuResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
