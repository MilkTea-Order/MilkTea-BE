using MilkTea.Domain.Respositories.Orders;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Results.Orders;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetTableEmptyUseCase(ITableRepository tabeleRepository)
    {
        private readonly ITableRepository _vTableRepository = tabeleRepository;

        public async Task<GetTableEmptyResult> Execute()
        {
            var result = new GetTableEmptyResult();
            var tables = await _vTableRepository.GetTableEmpty();
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
    }
}
