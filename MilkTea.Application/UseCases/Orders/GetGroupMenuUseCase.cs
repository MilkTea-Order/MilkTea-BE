using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetGroupMenuUseCase(IStatusRepository statusRepository, IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _menuRepository = menuRepository;
        public async Task<GetGroupMenuResult> Execute(GetGroupMenuQuery query)
        {
            GetGroupMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            if (query.StatusId.HasValue)
            {
                if (query.StatusId is <= 0) return SendMessageError(result, ErrorCode.E0036, "StatusID");
                var isExists = await _vStatusRepository.ExistsStatusAsync(query.StatusId.Value);
                if (!isExists) return SendMessageError(result, ErrorCode.E0001, "StatusID");
            }
            if (query.ItemStatusId.HasValue)
            {
                if (query.ItemStatusId is <= 0) return SendMessageError(result, ErrorCode.E0036, "ItemStatusID");
                var isExists = await _vStatusRepository.ExistsStatusAsync(query.ItemStatusId.Value);
                if (!isExists) return SendMessageError(result, ErrorCode.E0001, "ItemStatusID");
            }
            var groups = await _menuRepository.GetMenuGroupsByStatusAsync(query.StatusId, query.ItemStatusId);
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
