using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
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
        private readonly IMenuRepository _menuRepository = menuRepository;
        public async Task<GetMenuItemsOfGroupResult> Execute(GetMenuItemsAvailableOfGroupQuery query)
        {
            GetMenuItemsOfGroupResult result = new();
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            // Check GroupMenuID
            if (query.GroupId <= 0) return SendMessageError(result, ErrorCode.E0036, "GroupID");
            var groupMenu = await _menuRepository.GetMenuGroupByIDAndAvaliableAsync(query.GroupId);
            if (groupMenu == null) return SendMessageError(result, ErrorCode.E0036, "GroupID");

            //Get Menu Items
            var menus = await _menuRepository.GetMenusAvailableOfGroupAsync(query.GroupId);
            result.Menus = menus.Select(m => new MenuItemDto
            {
                MenuId = m.ID,
                MenuCode = m.Code,
                MenuName = m.Name,
                MenuGroupId = m.MenuGroupID,
                MenuGroupName = m.MenuGroup?.Name,
                StatusId = m.StatusID,
                StatusName = m.Status?.Name ?? "Không rõ"
            }).ToList();
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
