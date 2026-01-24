using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetGroupMenuAvaliableUseCase(
                                      IStatusRepository statusRepository,
                                      IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _menuRepository = menuRepository;
        public async Task<GetGroupMenuResult> Execute()
        {
            GetGroupMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            var groups = await _menuRepository.GetMenuGroupsAvailableAsync();
            result.GroupMenu = groups.Select(g => new MenuGroupDto
            {
                MenuGroupId = g.ID,
                MenuGroupName = g.Name,
                StatusId = g.StatusID,
                StatusName = g.Status?.Name ?? "Không rõ",
                Quantity = 0 // Will be calculated if needed
            }).ToList();
            return result;
        }
        private GetGroupMenuResult SendMessageError(
            GetGroupMenuResult result,
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
