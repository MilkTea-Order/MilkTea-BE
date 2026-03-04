using MilkTea.Application.Features.Catalog.Dtos;

namespace MilkTea.Application.Features.Catalog.Abstractions.Services
{
    public interface ITableService
    {
        //Task<TableDto?> GetTableAsync(int tableId, CancellationToken cancellationToken = default);
        Task<bool> IsTableInUsing(int tableId, CancellationToken cancellationToken = default);

        Task<List<TableDto>> GetTableAsync(IReadOnlyCollection<int> tableIds, CancellationToken cancellationToken = default);
    }
}
