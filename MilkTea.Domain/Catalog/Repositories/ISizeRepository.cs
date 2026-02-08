using MilkTea.Domain.Catalog.Entities.Size;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for size-related data operations.
/// </summary>
public interface ISizeRepository
{
    /// <summary>
    /// Gets a size by ID.
    /// </summary>
    Task<SizeEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all sizes.
    /// </summary>
    Task<List<SizeEntity>> GetAllAsync();


    /// <summary>
    /// Get sizes by their IDs.
    /// </summary>
    Task<IReadOnlyDictionary<int, SizeEntity>> GetByIdsAsync(IEnumerable<int> sizeIds, CancellationToken cancellationToken = default);
}
