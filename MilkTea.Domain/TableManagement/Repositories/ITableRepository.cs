using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Legacy repository interface for table operations (compatibility).
/// </summary>
public interface ITableRepository
{
    /// <summary>
    /// Gets a table by ID.
    /// </summary>
    Task<DinnerTable?> GetByIdAsync(int id);

    /// <summary>
    /// Gets tables by status.
    /// </summary>
    Task<List<DinnerTable>> GetTablesByStatusAsync(int? statusId);

    /// <summary>
    /// Gets empty tables.
    /// </summary>
    Task<List<DinnerTable>> GetTableEmpty();
}
