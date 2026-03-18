using MilkTea.Application.Features.Catalog.Models.Dtos.Table;

namespace MilkTea.Application.Features.Catalog.Abstractions.Queries
{
    public interface ITableQuery
    {
        /// <summary>
        /// Asynchronously retrieves a list of table data transfer objects filtered by status.
        /// </summary>
        /// <param name="statusID">The status identifier to filter tables. If null, retrieves all tables.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of TableDto objects.</returns>
        Task<List<TableDto>> GetTableAsync(int? statusID, CancellationToken cancellationToken = default);
    }
}
