using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Identity;

/// <summary>
/// Repository implementation for gender operations.
/// </summary>
public class GenderRepository(AppDbContext context) : IGenderRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    public async Task<Gender?> GetByIdAsync(int id)
    {
        return await _context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<Gender>> GetAllAsync()
    {
        return await _context.Genders
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsGenderAsync(int genderId)
    {
        return await _context.Genders
            .AsNoTracking()
            .AnyAsync(g => g.Id == genderId);
    }
}
