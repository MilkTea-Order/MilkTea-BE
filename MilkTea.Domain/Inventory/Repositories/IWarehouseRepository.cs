using MilkTea.Domain.Inventory.Entities;

namespace MilkTea.Domain.Inventory.Repositories;

public interface IWarehouseRepository
{
    /// <summary>
    /// Retrieves a warehouse entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the warehouse entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the warehouse entity if found;
    /// otherwise, null.</returns>
    Task<WarehouseEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves warehouse entities associated with the specified material IDs asynchronously.
    /// </summary>
    /// <param name="materialIds">A collection of material IDs to filter warehouse entities.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of matching warehouse entities.</returns>
    Task<List<WarehouseEntity>> GetActiveByMaterialIdsAsync(IEnumerable<int> materialIds, CancellationToken cancellationToken = default);
}
