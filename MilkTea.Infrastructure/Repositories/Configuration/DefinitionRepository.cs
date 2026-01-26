using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Configuration.Entities;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Configuration;

/// <summary>
/// Repository implementation for definition operations.
/// </summary>
public class DefinitionRepository(AppDbContext context) : IDefinitionRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<Definition?> GetByCodeAsync(string code)
    {
        return await _context.Definitions
            .AsNoTracking()
            .Include(d => d.DefinitionGroup)
            .FirstOrDefaultAsync(d => d.Code == code);
    }

    /// <inheritdoc/>
    public async Task<List<Definition>> GetByGroupIdAsync(int groupId)
    {
        return await _context.Definitions
            .AsNoTracking()
            .Include(d => d.DefinitionGroup)
            .Where(d => d.DefinitionGroupID == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<DefinitionGroup>> GetAllGroupsAsync()
    {
        return await _context.DefinitionGroups
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Definition?> GetCodePrefixBill()
    {
        // Assuming there's a definition group for "Bill" or "CodePrefix"
        // and a definition with code "BILL_PREFIX" or similar
        return await _context.Definitions
            .AsNoTracking()
            .Include(d => d.DefinitionGroup)
            .FirstOrDefaultAsync(d => d.Code == "BILL_PREFIX" || d.Code == "CodePrefixBill");
    }
}
