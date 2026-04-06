using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Features.Auth.Queries;

public class AuthQuery(
    AppDbContext context,
    IConfigurationService configurationService) : IAuthQuery
{
    private readonly AppDbContext _vContext = context;
    private readonly IConfigurationService _vConfigurationService = configurationService;

    public async Task<OtpEntity?> GetLatestOtpByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default)
    {
        return await _vContext.Otps
            .AsNoTracking()
            .Where(o => o.Email == email && o.OTPType == otpType)
            .OrderByDescending(o => o.OTPDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OtpEntity?> GetLatestActiveOtpByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default)
    {
        var otp = await GetLatestOtpByEmailAndTypeAsync(email, otpType, cancellationToken);
        if (otp == null) return null;

        var expirationMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);
        var maxAttempts = await _vConfigurationService.GetOtpMaxAttemptsAsync(cancellationToken);

        // Skip nếu đã expired hoặc đã vượt max attempts
        if (otp.IsExpired(expirationMinutes) || otp.IsMaxAttemptsReached(maxAttempts))
            return null;

        return otp;
    }
}
