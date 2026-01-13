using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetTableByStatusUseCase(IStatusRepository statusRepository, ITableRepository tableRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly ITableRepository _vTableRepository = tableRepository;
        public async Task<GetTableByStatusResult> Execute(GetTableByStatusCommand command)
        {

            GetTableByStatusResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            // Validate status ID
            if (command.statusID <= 0)
            {
                return SendMessageError(result, ErrorCode.E0029, "StatusID");
            }
            // Check exist tables with status ID
            if (command.statusID.HasValue)
            {
                var existsStatus = await _vStatusRepository.ExistsDinnerTableStatusAsync(command.statusID.Value);
                if (!existsStatus)
                {
                    return SendMessageError(result, ErrorCode.E0001, "StatusID");
                }
            }
            result.Tables = await _vTableRepository.GetTablesByStatusAsync(command.statusID);
            return result;
        }

        private GetTableByStatusResult SendMessageError(
            GetTableByStatusResult result,
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
