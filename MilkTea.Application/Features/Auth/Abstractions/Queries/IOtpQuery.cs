using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Application.Features.Auth.Abstractions.Queries;

public interface IOtpQuery
{
    /// <summary>
    /// Gets the latest valid OTP for a session (not expired).
    /// </summary>
    Task<OtpEntity?> GetLatestValidOtpBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an OTP by its ID without tracking.
    /// </summary>
    Task<OtpEntity?> GetOtpByIdAsync(int otpId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts successful OTP sends for a session and channel.
    /// If verifiedDate is provided, only counts OTPs created after that timestamp (for counter reset after verification).
    /// </summary>
    Task<int> CountSuccessfulOtpBySessionAndChannelAsync(int sessionId, string channel, DateTime? verifiedDate, CancellationToken cancellationToken = default);
}
