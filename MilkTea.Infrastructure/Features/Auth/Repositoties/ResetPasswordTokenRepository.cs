using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Repositoties;

public class ResetPasswordTokenRepository : IResetPasswordTokenRepository
{
    private readonly AppDbContext _vContext;

    public ResetPasswordTokenRepository(AppDbContext context)
    {
        _vContext = context;
    }

    /// <inheritdoc/>
    public async Task<ResetPasswordTokenEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.ResetPasswordTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ResetPasswordTokenEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.ResetPasswordTokens
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ResetPasswordTokenEntity?> GetValidTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _vContext.ResetPasswordTokens
            .AsNoTracking()
            .Where(t => t.Token == token)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ResetPasswordTokenEntity?> GetValidTokenForUpdateAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _vContext.ResetPasswordTokens
            .Where(t => t.Token == token)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(ResetPasswordTokenEntity token, CancellationToken cancellationToken = default)
    {
        await _vContext.ResetPasswordTokens.AddAsync(token, cancellationToken);
    }
}
