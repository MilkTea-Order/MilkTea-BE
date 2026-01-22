using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetTableEmptyUseCase(ITableRepository tabeleRepository)
    {
        private readonly ITableRepository _vTableRepository = tabeleRepository;

        public async Task<List<DinnerTable>> Execute()
        {
            return await _vTableRepository.GetTableEmpty();
        }
    }
}
