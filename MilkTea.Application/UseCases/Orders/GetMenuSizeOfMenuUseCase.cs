using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetMenuSizeOfMenuUseCase(IStatusRepository statusRepository,
                                            IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _vMenuRepository = menuRepository;

        public async Task<GetMenuSizeOfMenuResult> Execute(GetMenuSizeOfMenuCommand command)
        {
            GetMenuSizeOfMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            //Check MenuID
            if (command.MenuID is <= 0) return SendMessageError(result, ErrorCode.E0036, "MenuID");

            // Check xem còn bán hay không 
            var menu = await _vMenuRepository.GetMenuByIDAndAvaliableAsync(command.MenuID);
            if (menu == null) return SendMessageError(result, ErrorCode.E0040, "MenuID");

            result.MenuSize = await _vMenuRepository.GetMenuSizeWithPriceByMenuAsync(command.MenuID);
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