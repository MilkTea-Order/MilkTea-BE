using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for dinner table operations.
/// </summary>
public interface IDinnerTableRepository
{
    /// <summary>
    /// Gets a dinner table by ID.
    /// </summary>
    Task<DinnerTable?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all dinner tables.
    /// </summary>
    Task<List<DinnerTable>> GetAllAsync();

    /// <summary>
    /// Gets dinner tables by status.
    /// </summary>
    Task<List<DinnerTable>> GetByStatusAsync(DinnerTableStatus status);

    /// <summary>
    /// Gets all empty tables.
    /// </summary>
    Task<List<DinnerTable>> GetEmptyTablesAsync();

    /// <summary>
    /// Updates a dinner table.
    /// </summary>
    Task<bool> UpdateAsync(DinnerTable table);

    /// <summary>
    /// Gets a table by ID (alias for GetByIdAsync for compatibility).
    /// </summary>
    Task<DinnerTable?> GetTableByIdAsync(int id);
}
