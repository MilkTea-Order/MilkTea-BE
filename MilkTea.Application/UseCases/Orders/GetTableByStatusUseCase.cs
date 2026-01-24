using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Queries.Orders;
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
        public async Task<GetTableByStatusResult> Execute(GetTableByStatusQuery query)
        {

            GetTableByStatusResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

            // Validate status ID
            if (query.StatusId <= 0)
            {
                return SendMessageError(result, ErrorCode.E0029, "StatusID");
            }
            // Check exist tables with status ID
            if (query.StatusId.HasValue)
            {
                var existsStatus = await _vStatusRepository.ExistsDinnerTableStatusAsync(query.StatusId.Value);
                if (!existsStatus)
                {
                    return SendMessageError(result, ErrorCode.E0001, "StatusID");
                }
            }
            var tables = await _vTableRepository.GetTablesByStatusAsync(query.StatusId);
            result.Tables = tables.Select(static t => new DinnerTableDto
            {
                Id = t.ID,
                Code = t.Code,
                Name = t.Name,
                Position = t.Position,
                NumberOfSeats = t.NumberOfSeats,
                StatusId = t.StatusOfDinnerTableID,
                StatusName = t.StatusOfDinnerTable?.Name,
                Note = t.Note
            }).ToList();
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
