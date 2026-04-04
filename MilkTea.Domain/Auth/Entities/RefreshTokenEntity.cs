using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

public sealed class RefreshTokenEntity : Entity<int>
{
    public int UserId { get; private set; }
    public string Token { get; private set; } = default!;
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }

    public bool IsExpired => DateTime.Now >= ExpiryDate;
    public bool IsValid => !IsRevoked && !IsExpired;

    private RefreshTokenEntity() { }

    internal static RefreshTokenEntity Create(int userId, string token, DateTime expiryDate, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (expiryDate <= DateTime.Now)
        {
            throw new ArgumentException("Expiry date must be in the future.", nameof(expiryDate));
        }

        var now = DateTime.Now;

        return new RefreshTokenEntity
        {
            UserId = userId,
            Token = token,
            ExpiryDate = expiryDate,
            IsRevoked = false,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }


    internal void Revoke(int revokedBy)
    {
        if (IsRevoked)
        {
            throw new InvalidOperationException("Refresh token is already revoked.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(revokedBy);

        IsRevoked = true;
        UpdatedBy = revokedBy;
        UpdatedDate = DateTime.Now;
    }
}
