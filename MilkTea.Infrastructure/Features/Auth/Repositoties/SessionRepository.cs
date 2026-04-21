using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Repositoties;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _vContext;

    public SessionRepository(AppDbContext context)
    {
        _vContext = context;
    }

    /// <inheritdoc/>
    public async Task<SessionEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SessionEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Sessions
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SessionEntity?> GetLatestActiveSessionByEmailAndFunctionAsync(string email, string function, CancellationToken cancellationToken = default)
    {
        return await _vContext.Sessions
            .AsNoTracking()
            .Where(s => s.Email == email && s.Function == function && s.Status == "PENDING")
            .OrderByDescending(s => s.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SessionEntity?> GetLatestSessionByEmailAndFunctionAsync(string email, string function, CancellationToken cancellationToken = default)
    {
        return await _vContext.Sessions
            .AsNoTracking()
            .Where(s => s.Email == email && s.Function == function)
            .OrderByDescending(s => s.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(SessionEntity session, CancellationToken cancellationToken = default)
    {
        await _vContext.Sessions.AddAsync(session, cancellationToken);
    }
}
