using MilkTea.Application.Commands.Orders;
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
        private readonly IMenuRepository _vMenuRepository = menuRepository;
        public async Task<GetGroupMenuResult> Execute(GetGroupMenuCommnad command)
        {
            GetGroupMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            if (command.StatusID.HasValue)
            {
                if (command.StatusID is <= 0) return SendMessageError(result, ErrorCode.E0036, "StatusID");
                var isExists = await _vStatusRepository.ExistsStatusAsync(command.StatusID.Value);
                if (!isExists) return SendMessageError(result, ErrorCode.E0001, "StatusID");
            }
            if (command.ItemStatusID.HasValue)
            {
                if (command.ItemStatusID is <= 0) return SendMessageError(result, ErrorCode.E0036, "ItemStatusID");
                var isExists = await _vStatusRepository.ExistsStatusAsync(command.ItemStatusID.Value);
                if (!isExists) return SendMessageError(result, ErrorCode.E0001, "ItemStatusID");
            }
            result.GroupMenu = await _vMenuRepository.GetMenuGroupByStatusAsync(command.StatusID, command.ItemStatusID);
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
