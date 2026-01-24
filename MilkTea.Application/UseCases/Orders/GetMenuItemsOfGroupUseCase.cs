using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
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
        private readonly IMenuRepository _menuRepository = menuRepository;
        public async Task<GetMenuItemsOfGroupResult> Execute(GetMenuItemsOfGroupQuery query)
        {
            GetMenuItemsOfGroupResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            if (query.GroupId is <= 0) return SendMessageError(result, ErrorCode.E0036, "GroupID");
            if (query.MenuStatusId.HasValue)
            {
                if (query.MenuStatusId.Value is <= 0) return SendMessageError(result, ErrorCode.E0036, "MenuStatusID");
                var isExist = await _vStatusRepository.ExistsStatusAsync(query.MenuStatusId.Value);
                if (!isExist) return SendMessageError(result, ErrorCode.E0001, "MenuStatusID");
            }
            var menus = await _menuRepository.GetMenusOfGroupByStatusAsync(query.GroupId, query.MenuStatusId);
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
