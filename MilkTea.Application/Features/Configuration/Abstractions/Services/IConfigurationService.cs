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
    }
}
