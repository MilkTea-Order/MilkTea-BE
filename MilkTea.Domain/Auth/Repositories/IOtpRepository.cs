using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Domain.Auth.Repositories;

public interface IOtpRepository
{
    /// <summary>
    /// Gets an OTP entity by its ID.
    /// </summary>
    Task<OtpEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an OTP entity by its ID with tracking enabled for updates.
    /// </summary>
    Task<OtpEntity?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest OTP for an email address and OTP type.
    /// </summary>
    Task<OtpEntity?> GetLatestByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new OTP entity to the database.
    /// </summary>
    Task AddAsync(OtpEntity otp, CancellationToken cancellationToken = default);
}
