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
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<Size?> GetByIdAsync(int id)
    {
        return await _vContext.Sizes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<Size>> GetAllAsync()
    {
        return await _vContext.Sizes
            .AsNoTracking()
            .OrderBy(s => s.RankIndex)
            .ToListAsync();
    }


    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<int, Size>> GetByIdsAsync(
        IEnumerable<int> sizeIds,
        CancellationToken cancellationToken = default)
    {
        if (sizeIds is null) return new Dictionary<int, Size>();

        var ids = sizeIds
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (ids.Length == 0) return new Dictionary<int, Size>();

        var sizes = await _vContext.Sizes
            .AsNoTracking()
            .Where(s => ids.Contains(s.Id))
            .ToListAsync(cancellationToken);

        return sizes.ToDictionary(s => s.Id, s => s);
    }

}
