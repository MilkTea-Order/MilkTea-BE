using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetMenuItemsAvaliableOfGroupUseCase(
                                        IStatusRepository statusRepository,
                                        IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _vMenuRepository = menuRepository;
        public async Task<GetMenuItemsOfGroupResult> Execute(GetMenuItemsAvaliableOfGroupCommand command)
        {
            GetMenuItemsOfGroupResult result = new();
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            // Check GroupMenuID
            if (command.GroupID <= 0) return SendMessageError(result, ErrorCode.E0036, "GroupID");
            var groupMenu = await _vMenuRepository.GetMenuGroupByIDAndAvaliableAsync(command.GroupID);
            if (groupMenu == null) return SendMessageError(result, ErrorCode.E0036, "GroupID");

            //Get Menu Items
            result.Menus = await _vMenuRepository.GetMenuItemsAvaliableOfGroupByStatusAsync(command.GroupID);
            return result;
        }
        private GetMenuItemsOfGroupResult SendMessageError(
            GetMenuItemsOfGroupResult result,
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
