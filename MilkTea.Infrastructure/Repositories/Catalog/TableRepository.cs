using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

public class TableRepository(AppDbContext context) : ITableRepository
{
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<TableEntity?> GetByIdAsync(int id)
    {
        return await _vContext.Tables
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public async Task<TableEntity?> GetByIdForUpdateAsync(int id)
    {
        return await _vContext.Tables
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<TableEntity>> GetAllAsync()
    {
        return await _vContext.Tables
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<TableEntity>> GetByStatusAsync(TableStatus status)
    {
        return await _vContext.Tables
            .AsNoTracking()
            .Where(t => t.Status == status)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<TableEntity>> GetUsingTablesAsync()
    {
        return await _vContext.Tables
            .AsNoTracking()
            .Where(t => t.Status == TableStatus.InUsing)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TableEntity>> GetTableAsync(bool isEmpty = true, CancellationToken cancellationToken = default)
    {
        var query = _vContext.Tables
        .AsNoTracking()
        .Where(t => t.Status == TableStatus.InUsing);

        if (isEmpty)
        {
            query = query.Where(t => !_vContext.Orders.AsNoTracking().Any(o =>
                o.DinnerTableId == t.Id &&
                o.Status == Domain.Orders.Enums.OrderStatus.Unpaid));
        }
        else
        {
            query = query.Where(t => _vContext.Orders.AsNoTracking().Any(o =>
                o.DinnerTableId == t.Id &&
                o.Status == Domain.Orders.Enums.OrderStatus.Unpaid));
        }

        return await query.OrderBy(t => t.Name).ToListAsync(cancellationToken);
    }
}
