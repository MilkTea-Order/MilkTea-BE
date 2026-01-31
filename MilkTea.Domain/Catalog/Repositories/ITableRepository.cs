using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for dinner table operations.
/// </summary>
public interface ITableRepository
{
    /// <summary>
    /// Gets a dinner table by ID.
    /// </summary>
    Task<TableEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all dinner tables.
    /// </summary>
    Task<List<TableEntity>> GetAllAsync();

    /// <summary>
    /// Gets dinner tables by status.
    /// </summary>
    Task<List<TableEntity>> GetByStatusAsync(TableStatus status);

    /// <summary>
    /// Gets all empty tables.
    /// </summary>
    Task<List<TableEntity>> GetUsingTablesAsync();

    /// <summary>
    /// Gets a table by ID (alias for GetByIdAsync for compatibility).
    /// </summary>
    Task<TableEntity?> GetByIdForUpdateAsync(int id);


    /// <summary>
    /// Default get table empty
    /// Gets empty tables where:
    /// - Table status is InUsing
    /// - No orders with unpaid or not-collected status are associated with the table
    /// Get not empty table is otherwise;
    /// </summary>
    Task<IReadOnlyList<TableEntity>> GetTableEmptyAsync(bool isEmpty = true, CancellationToken cancellationToken = default);
}
