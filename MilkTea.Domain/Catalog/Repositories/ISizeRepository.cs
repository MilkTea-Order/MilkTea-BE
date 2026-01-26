using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for size-related data operations.
/// </summary>
public interface ISizeRepository
{
    /// <summary>
    /// Gets a size by ID.
    /// </summary>
    Task<Size?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all sizes.
    /// </summary>
    Task<List<Size>> GetAllAsync();
}
