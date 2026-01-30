using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Users.Entities;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users;

/// <summary>
/// Entity Framework Core implementation of <see cref="IRoleRepository"/>.
/// Repositories are read-only and expose GET methods only.
/// </summary>
public class RoleRepository(AppDbContext context) : IRoleRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc/>
    /// <remarks>
    /// Aggregate-only GET.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </remarks>
    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Aggregate-only GET (collection).
    /// Retrieves all roles without loading related entities.
    /// Uses AsNoTracking() for read-only queries.
    /// </remarks>
    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Aggregate-with-relations GET.
    /// Loads roles assigned to the specified user via the `UserRoles` junction table.
    /// Includes no other related aggregates.
    /// </remarks>
    public async Task<List<Role>> GetRolesByUserIdAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserID == userId)
            .Join(
                _context.Roles,
                ur => ur.RoleID,
                r => r.Id,
                (_, r) => r)
            .AsNoTracking()
            .Distinct()
            .ToListAsync();
    }
}

