using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Refresh token entity for JWT authentication.
/// Child entity of User aggregate.
/// </summary>
public sealed class RefreshToken : Entity<int>
{
    public int UserId { get; private set; }
    public User? User { get; private set; }

    public string Token { get; private set; } = default!;
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }

    // For EF Core
    private RefreshToken() { }

    internal static RefreshToken Create(int userId, string token, DateTime expiryDate)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        if (expiryDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiry date must be in the future.", nameof(expiryDate));

        var now = DateTime.UtcNow;

        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiryDate = expiryDate,
            IsRevoked = false,
            CreatedBy = userId,
            CreatedDate = now
        };
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsValid => !IsRevoked && !IsExpired;

    public void Revoke(int revokedBy)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Refresh token is already revoked.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(revokedBy);

        IsRevoked = true;
        UpdatedDate = DateTime.UtcNow;
    }
}
