using MilkTea.Domain.Auth.Entities;

namespace MilkTea.Application.Features.Auth.Abstractions.Queries
{
    public interface IAuthQuery
    {
        Task<OtpEntity?> GetLatestOtpByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the latest active OTP (not expired and not exceeded max attempts) for an email and type.
        /// </summary>
        Task<OtpEntity?> GetLatestActiveOtpByEmailAndTypeAsync(string email, string otpType, CancellationToken cancellationToken = default);
    }
}
