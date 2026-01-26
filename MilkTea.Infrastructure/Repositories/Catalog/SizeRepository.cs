using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

/// <summary>
/// Repository implementation for size-related data operations.
/// </summary>
public class SizeRepository(AppDbContext context) : ISizeRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<Size?> GetByIdAsync(int id)
    {
        return await _context.Sizes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<Size>> GetAllAsync()
    {
        return await _context.Sizes
            .AsNoTracking()
            .OrderBy(s => s.RankIndex)
            .ToListAsync();
    }
}
