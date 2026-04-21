using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Repositoties;

public class OtpRepository : IOtpRepository
{
    private readonly AppDbContext _vContext;

    public OtpRepository(AppDbContext context)
    {
        _vContext = context;
    }

    /// <inheritdoc/>
    public async Task<OtpEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Otps
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OtpEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _vContext.Otps
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(OtpEntity otp, CancellationToken cancellationToken = default)
    {
        await _vContext.Otps.AddAsync(otp, cancellationToken);
    }
}
