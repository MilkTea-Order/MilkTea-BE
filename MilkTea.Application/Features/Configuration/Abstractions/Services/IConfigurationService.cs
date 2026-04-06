namespace MilkTea.Application.Features.Configuration.Abstractions.Services
{
    public interface IConfigurationService
    {
        Task<string?> GetBillPrefix();
        Task<bool> IsWarehouseManagementMode(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the OTP expiration time in minutes.
        /// Returns default value of 5 minutes if not configured.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>OTP expiration time in minutes.</returns>
        Task<int> GetOtpExpirationMinutesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the maximum number of OTP attempts.
        /// Returns default value of 5 if not configured.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Maximum OTP attempts.</returns>
        Task<int> GetOtpMaxAttemptsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the reset password token expiration time in minutes.
        /// Returns default value of 5 minutes if not configured.
        /// </summary>
        Task<int> GetResetPasswordTokenExpirationMinutesAsync(CancellationToken cancellationToken = default);
    }
}
