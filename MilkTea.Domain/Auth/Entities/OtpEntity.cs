using MilkTea.Domain.Auth.ValueObjects;
using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public sealed class OtpEntity : Entity<int>
{
    public int SessionId { get; private set; }
    public Channel Channel { get; private set; }
    public string OtpCode { get; private set; } = null!;
    public DateTime ExpiredDate { get; private set; }
    public OtpStatus Status { get; private set; } = OtpStatus.Default;

    /// <summary>
    /// Checks if the OTP is expired based on ExpiredDate.
    /// </summary>
    public bool IsExpired => DateTime.Now > ExpiredDate;

    /// <summary>
    /// Checks if the OTP is valid (not expired and not exceeded max attempts).
    /// </summary>
    public bool IsValid() => !IsExpired;

    private OtpEntity() { }

    /// <summary>
    /// Creates a new OTP entity with auto-generated 6-digit OTP code.
    /// </summary>
    /// <param name="sessionId">The ID of the session this OTP belongs to.</param>
    /// <param name="channel">The channel through which the OTP was sent.</param>
    /// <param name="expiredDate">The date/time when the OTP expires.</param>
    /// <returns>A new OtpEntity instance with auto-generated OTP code.</returns>
    public static OtpEntity Create(int sessionId, Channel channel, DateTime expiredDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(channel.ToString());

        return new OtpEntity
        {
            SessionId = sessionId,
            Channel = channel,
            OtpCode = GenerateOtpCode(),
            ExpiredDate = expiredDate,
            CreatedDate = DateTime.Now
        };
    }

    /// <summary>
    /// Generates a random 6-digit OTP code.
    /// </summary>
    public static string GenerateOtpCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
