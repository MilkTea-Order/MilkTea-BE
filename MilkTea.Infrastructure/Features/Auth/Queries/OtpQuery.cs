using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.ValueObjects;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Queries;

public class OtpQuery(AppDbContext context) : IOtpQuery
{
    private readonly AppDbContext _vContext = context;

    public async Task<OtpEntity?> GetLatestValidOtpBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        return await _vContext.Otps
            .AsNoTracking()
            .Where(o => o.SessionId == sessionId && o.Status == OtpStatus.Success && o.ExpiredDate > DateTime.Now)
            .OrderByDescending(o => o.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OtpEntity?> GetOtpByIdAsync(int otpId, CancellationToken cancellationToken = default)
    {
        return await _vContext.Otps
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == otpId, cancellationToken);
    }

    public async Task<int> CountSuccessfulOtpBySessionAndChannelAsync(int sessionId, string channel, DateTime? verifiedDate, CancellationToken cancellationToken = default)
    {
        var channelValue = channel.ToUpperInvariant();
        var query = _vContext.Otps
            .AsNoTracking()
            .Where(o => o.SessionId == sessionId && o.Status == OtpStatus.Success && o.Channel == channelValue);

        if (verifiedDate.HasValue)
        {
            // Session verified: count only OTPs created AFTER verification (reset counter)
            query = query.Where(o => o.CreatedDate > verifiedDate.Value);
        }
        // If not verified, count all successful OTPs

        return await query.CountAsync(cancellationToken);
    }
}
