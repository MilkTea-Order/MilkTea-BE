using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// Child entity of User aggregate. Cannot be created or revoked outside User aggregate.
/// </summary>
public sealed class RefreshToken : Entity<int>
{
    public int UserId { get; private set; }
    public string Token { get; private set; } = default!;
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsValid => !IsRevoked && !IsExpired;

    // EF Core requires parameterless constructor
    private RefreshToken() { }

    /// <summary>
    /// Internal factory method. Only User aggregate can create RefreshToken.
    /// </summary>
    internal static RefreshToken Create(int userId, string token, DateTime expiryDate, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (expiryDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiry date must be in the future.", nameof(expiryDate));

        var now = DateTime.UtcNow;

        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiryDate = expiryDate,
            IsRevoked = false,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    /// <summary>
    /// Internal method. Only User aggregate can revoke RefreshToken.
    /// </summary>
    internal void Revoke(int revokedBy)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Refresh token is already revoked.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(revokedBy);

        IsRevoked = true;
        UpdatedBy = revokedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
