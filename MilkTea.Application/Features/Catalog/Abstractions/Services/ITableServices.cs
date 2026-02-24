namespace MilkTea.Application.Features.Catalog.Abstractions.Services
{
    public interface ITableServices
    {
        //Task<TableDto?> GetTableAsync(int tableId, CancellationToken cancellationToken = default);

        //Task<Dictionary<int, TableDto>> GetTableAsync(IReadOnlyCollection<int> tableIds, CancellationToken cancellationToken = default);
        Task<bool> IsTableInUsing(int tableId, CancellationToken cancellationToken = default);
    }
}
