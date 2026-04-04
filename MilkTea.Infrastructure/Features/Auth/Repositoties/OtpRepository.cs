using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Repositoties;

public class OtpRepository : IOtpRepository
{
    private readonly AppDbContext _context;

    public OtpRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<OtpEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Otps
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OtpEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Otps
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OtpEntity?> GetLatestByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await _context.Otps
            .AsNoTracking()
            .Where(o => o.Email != null && o.Email.ToLower() == normalizedEmail && o.OTPType == otpType)
            .OrderByDescending(o => o.OTPDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(OtpEntity otp, CancellationToken cancellationToken = default)
    {
        await _context.Otps.AddAsync(otp, cancellationToken);
    }
}
