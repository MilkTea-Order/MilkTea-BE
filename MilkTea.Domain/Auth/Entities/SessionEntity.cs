using MilkTea.Domain.Auth.ValueObjects;
using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public sealed class SessionEntity : Entity<int>
{
    public string Email { get; private set; } = null!;

    /// <summary>
    /// The channel through which the OTP was sent (e.g., Email, SMS).
    /// </summary>
    public Channel Channel { get; private set; }

    /// <summary>
    /// The function/action this session is for (VALIDATE_CUSTOMER, REGISTER, VALIDATE_EMAIL, VALIDATE_PHONE, RESET_PASSWORD, LOGIN).
    /// </summary>
    public SessionFunction Function { get; private set; }

    /// <summary>
    /// The status of the session (e.g., PENDING, VERIFIED, EXPIRED).
    /// </summary>
    public SessionStatus Status { get; private set; } = SessionStatus.Default;

    /// <summary>
    /// The date/time when the session expires.
    /// </summary>
    public DateTime ExpiresDate { get; private set; }

    /// <summary>
    /// The date/time when the session was verified.
    /// </summary>
    public DateTime? VerifiedDate { get; private set; } = null!;

    /// <summary>
    /// Whether the session is verified (VerifiedDate is set).
    /// </summary>
    public bool IsVerified => VerifiedDate.HasValue;

    /// <summary>
    /// Checks if the session has expired.
    /// </summary>
    public bool IsExpired => DateTime.Now > ExpiresDate;

    private SessionEntity() { }

    /// <summary>
    /// Creates a new Session entity.
    /// </summary>
    /// <param name="email">The email address associated with this session.</param>
    /// <param name="channel">The channel through which the OTP was sent.</param>
    /// <param name="function">The function/action this session is for.</param>
    /// <param name="expiresDate">The date/time when the session expires.</param>
    /// <returns>A new SessionEntity instance.</returns>
    public static SessionEntity Create(string email,
                                        Channel channel,
                                        SessionFunction function,
                                        DateTime expiresDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var now = DateTime.Now;

        return new SessionEntity
        {
            Email = email.Trim().ToLowerInvariant(),
            Channel = channel,
            Function = function,
            ExpiresDate = expiresDate,
            CreatedDate = now
        };
    }

    /// <summary>
    /// Marks the session as verified by setting the VerifiedDate.
    /// </summary>
    /// <param name="verifiedBy">The ID of the user or system that verified this session.</param>
    public void MarkAsVerified(int? verifiedBy = null)
    {
        if (IsVerified)
        {
            throw new InvalidOperationException("Session is already verified.");
        }
        VerifiedDate = DateTime.Now;
        Status = SessionStatus.Verified;
    }
}
