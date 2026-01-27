using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Entity Framework Core implementation of <see cref="IGenderRepository"/>.
/// Provides data access operations for Gender lookup entity using EF Core.
/// Gender is a reference data entity used for employee profile information.
/// </summary>
public class GenderRepository(AppDbContext context) : IGenderRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    /// <remarks>
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<Gender?> GetByIdAsync(int id)
    {
        return await _context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Retrieves all genders from the database.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// Typically used for populating dropdown lists or validation.
    /// </remarks>
    public async Task<List<Gender>> GetAllAsync()
    {
        return await _context.Genders
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Checks if a gender with the specified ID exists.
    /// Uses AsNoTracking() for read-only queries.
    /// Used for validation before creating or updating employee records.
    /// </remarks>
    public async Task<bool> ExistsGenderAsync(int genderId)
    {
        return await _context.Genders
            .AsNoTracking()
            .AnyAsync(g => g.Id == genderId);
    }
}
