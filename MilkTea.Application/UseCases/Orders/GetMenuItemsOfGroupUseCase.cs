using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetMenuItemsOfGroupUseCase(IStatusRepository statusRepository, IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _vMenuRepository = menuRepository;
        public async Task<GetMenuItemsOfGroupResult> Execute(GetMenuItemsOfGroupCommand command)
        {
            GetMenuItemsOfGroupResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            if (command.GroupID is <= 0) return SendMessageError(result, ErrorCode.E0036, "GroupID");
            if (command.MenuStatusID.HasValue)
            {
                if (command.MenuStatusID.Value is <= 0) return SendMessageError(result, ErrorCode.E0036, "MenuStatusID");
                var isExist = await _vStatusRepository.ExistsStatusAsync(command.MenuStatusID.Value);
                if (!isExist) return SendMessageError(result, ErrorCode.E0001, "MenuStatusID");
            }
            result.Menus = await _vMenuRepository.GetMenuItemsOfGroupByStatusAsync(command.GroupID, command.MenuStatusID);
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
