using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;


public sealed class OtpEntity : Entity<int>
{
    /// <summary>
    /// The email address the OTP was sent to.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// The OTP code value.
    /// </summary>
    public string OtpCode { get; private set; } = null!;

    /// <summary>
    /// The date/time when the OTP was created.
    /// </summary>
    public DateTime OTPDate { get; private set; }

    /// <summary>
    /// The type of OTP (e.g., CHANGE_PASSWORD, FORGOT_PASSWORD).
    /// </summary>
    public string OTPType { get; private set; } = null!;

    /// <summary>
    /// Number of times the OTP has been used.
    /// </summary>
    public int NumOfTimes { get; private set; }

    /// <summary>
    /// Checks if the OTP is expired based on OTPDate and configured expiration time.
    /// </summary>
    public bool IsExpired(int expirationMinutes) => DateTime.Now > OTPDate.AddMinutes(expirationMinutes);

    /// <summary>
    /// Checks if the OTP has exceeded the maximum number of attempts.
    /// </summary>
    public bool IsMaxAttemptsReached(int maxAttempts) => NumOfTimes >= maxAttempts;

    /// <summary>
    /// Checks if the OTP is valid (not expired and not exceeded max attempts).
    /// </summary>
    public bool IsValid(int expirationMinutes, int maxAttempts) =>
        !IsExpired(expirationMinutes) && !IsMaxAttemptsReached(maxAttempts);

    private OtpEntity() { }

    /// <summary>
    /// Creates a new OTP entity with auto-generated 6-digit OTP code.
    /// </summary>
    /// <param name="email">The email address to send the OTP to.</param>
    /// <param name="otpType">The type of OTP (e.g., CHANGE_PASSWORD, FORGOT_PASSWORD).</param>
    /// <param name="createdBy">The ID of the user or system that created this OTP.</param>
    /// <returns>A new OtpEntity instance with auto-generated OTP code.</returns>
    public static OtpEntity Create(string? email, string otpType, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(otpType);

        return new OtpEntity
        {
            Email = email?.Trim().ToLowerInvariant(),
            OtpCode = GenerateOtpCode(),
            OTPDate = DateTime.Now,
            OTPType = otpType,
            NumOfTimes = 1,
            CreatedBy = createdBy,
            CreatedDate = DateTime.Now
        };
    }

    /// <summary>
    /// Generates a random 6-digit OTP code.
    /// </summary>
    private static string GenerateOtpCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    /// <summary>
    /// Increments the number of times the OTP has been used.
    /// </summary>
    public void IncrementNumOfTimes()
    {
        NumOfTimes++;
        UpdatedDate = DateTime.Now;
    }
}
