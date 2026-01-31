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
    private readonly AppDbContext _vContext = context;

    /// <inheritdoc/>
    public async Task<Definition?> GetByCodeAsync(string code)
    {
        return await _vContext.Definitions
            .AsNoTracking()
            .Include(d => d.DefinitionGroupID)
            .FirstOrDefaultAsync(d => d.Code == code);
    }

    /// <inheritdoc/>
    public async Task<List<Definition>> GetByGroupIdAsync(int groupId)
    {
        return await _vContext.Definitions
            .AsNoTracking()
            .Where(d => d.DefinitionGroupID == groupId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<DefinitionGroup>> GetAllGroupsAsync()
    {
        return await _vContext.DefinitionGroups
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Definition?> GetCodePrefixBill()
    {
        Console.WriteLine("Test nè");
        return await _vContext.Definitions
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Code == "BILL_PREFIX" || d.Code == "CodePrefixBill");
    }
}
