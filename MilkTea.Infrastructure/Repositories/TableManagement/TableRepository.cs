using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.TableManagement;

/// <summary>
/// Legacy repository implementation for table operations (compatibility).
/// </summary>
public class TableRepository(AppDbContext context) : ITableRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<DinnerTable?> GetByIdAsync(int id)
    {
        return await _context.DinnerTables
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<DinnerTable>> GetTablesByStatusAsync(int? statusId)
    {
        var query = _context.DinnerTables.AsNoTracking();

        if (statusId.HasValue)
        {
            var status = (DinnerTableStatus)statusId.Value;
            query = query.Where(t => t.Status == status);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<DinnerTable>> GetTableEmpty()
    {
        return await _context.DinnerTables
            .AsNoTracking()
            .Where(t => t.Status == DinnerTableStatus.Empty)
            .ToListAsync();
    }
}
