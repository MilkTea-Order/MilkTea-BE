using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetMenuSizeOfMenuUseCase(IStatusRepository statusRepository,
                                            IMenuRepository menuRepository,
                                            IPriceListRepository priceListRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _menuRepository = menuRepository;
        private readonly IPriceListRepository _priceListRepository = priceListRepository;

        public async Task<GetMenuSizeOfMenuResult> Execute(GetMenuSizeOfMenuQuery query)
        {
            GetMenuSizeOfMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            //Check MenuID
            if (query.MenuId is <= 0) return SendMessageError(result, ErrorCode.E0036, "MenuID");

            // Check xem còn bán hay không 
            var menu = await _menuRepository.GetMenuByIDAndAvaliableAsync(query.MenuId);
            if (menu == null) return SendMessageError(result, ErrorCode.E0040, "MenuID");

            var activePriceList = await _priceListRepository.GetActivePriceListAsync();
            if (activePriceList == null)
            {
                result.MenuSize = new List<MenuSizePriceDto>();
                return result;
            }

            // Chỉ lấy những menu size còn được bán (có trong active price list)
            var menuSizes = await _menuRepository.GetMenuSizesAvailableByMenuAsync(query.MenuId);
            var prices = await _priceListRepository.GetPricesForMenuAsync(activePriceList.ID, query.MenuId);
            
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

        private GetMenuSizeOfMenuResult SendMessageError(
            GetMenuSizeOfMenuResult result,
            string errorCode,
            params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}

//public async Task<GetMenuSizeOfMenuResult> Execute(GetMenuSizeOfMenuCommand command)
//{
//    GetMenuSizeOfMenuResult result = new();
//    // Set time
//    result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

//    //Check groupID
//    if (command.MenuID is <= 0)
//    {
//        return SendMessageError(result, ErrorCode.E0036, "MenuID");
//    }

//    // Check xem còn bán hay không 
//    var menu = await _vMenuRepository.GetMenuByIDAsync(command.MenuID);
//    if (menu == null)
//    {
//        return SendMessageError(result, ErrorCode.E0001, "MenuID");
//    }

//    var statusAvaliable = await _vStatusRepository.GetActive();
//    if (statusAvaliable == null)
//    {
//        return SendMessageError(result, ErrorCode.E0001, "Status");
//    }

//    if (statusAvaliable.ID != menu.StatusID)
//    {
//        return SendMessageError(result, ErrorCode.E0040, "MenuID");

//    }
//    result.MenuSize = await _vMenuRepository.GetMenuSizeWithPriceByMenuAsync(command.MenuID);
//    return result;
//}