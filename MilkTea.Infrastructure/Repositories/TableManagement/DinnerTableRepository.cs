using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.TableManagement;

/// <summary>
/// Repository implementation for dinner table operations.
/// </summary>
public class DinnerTableRepository(AppDbContext context) : IDinnerTableRepository
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
    public async Task<List<DinnerTable>> GetAllAsync()
    {
        return await _context.DinnerTables
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<DinnerTable>> GetByStatusAsync(DinnerTableStatus status)
    {
        return await _context.DinnerTables
            .AsNoTracking()
            .Where(t => t.Status == status)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<DinnerTable>> GetEmptyTablesAsync()
    {
        return await _context.DinnerTables
            .AsNoTracking()
            .Where(t => t.Status == DinnerTableStatus.Empty)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(DinnerTable table)
    {
        _context.DinnerTables.Update(table);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<DinnerTable?> GetTableByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }
}
