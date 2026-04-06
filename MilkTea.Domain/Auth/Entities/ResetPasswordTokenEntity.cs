using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Auth.Entities;

/// <summary>
/// Aggregate lưu trữ reset password token (JWT) dùng cho xác minh quên mật khẩu.
/// Token được tạo bởi JWT service và lưu vào DB để xác thực khi verify.
/// </summary>
public sealed class ResetPasswordTokenEntity : Aggregate<int>
{
    /// <summary>
    /// ID của user sở hữu token.
    /// </summary>
    public int UserId { get; private set; }

    /// <summary>
    /// Reset password token (JWT) đã được mã hóa.
    /// </summary>
    public string Token { get; private set; } = default!;

    /// <summary>
    /// Thời điểm token hết hạn.
    /// </summary>
    public DateTime ExpiryDate { get; private set; }

    /// <summary>
    /// Token đã được sử dụng (verify) hay chưa.
    /// </summary>
    public bool IsUsed { get; private set; } = false;

    /// <summary>
    /// Token đã bị vô hiệu hóa hay chưa.
    /// </summary>
    public bool IsRevoked { get; private set; } = false;

    /// <summary>
    /// Thời điểm token được sử dụng (verify thành công).
    /// </summary>
    public DateTime? UsedAt { get; private set; } = null;

    /// <summary>
    /// Kiểm tra token có hết hạn chưa.
    /// </summary>
    public bool IsExpired => DateTime.Now >= ExpiryDate;

    /// <summary>
    /// Kiểm tra token có hợp lệ không (chưa dùng và chưa hết hạn và chưa bị vô hiệu hóa).
    /// </summary>
    public bool IsValid => !IsUsed && !IsExpired && !IsRevoked;

    private ResetPasswordTokenEntity() { }

    /// <summary>
    /// Tạo mới một ResetPasswordTokenEntity.
    /// </summary>
    /// <param name="userId">ID của user.</param>
    /// <param name="token">JWT token đã được mã hóa.</param>
    /// <param name="expiresAt">Thời điểm hết hạn.</param>
    /// <returns>Instance mới của ResetPasswordTokenEntity.</returns>
    public static ResetPasswordTokenEntity Create(
        int userId,
        string token,
        DateTime expiresAt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        if (expiresAt <= DateTime.Now)
        {
            throw new ArgumentException("Expiry date must be in the future.", nameof(expiresAt));
        }

        return new ResetPasswordTokenEntity
        {
            UserId = userId,
            Token = token,
            ExpiryDate = expiresAt,
            CreatedDate = DateTime.Now,
        };
    }

    /// <summary>
    /// Đánh dấu token đã được sử dụng (verify thành công).
    /// </summary>
    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("Reset password token has already been used.");
        }

        if (IsExpired)
        {
            throw new InvalidOperationException("Reset password token has expired.");
        }

        IsUsed = true;
        UsedAt = DateTime.Now;
    }

    /// <summary>
    /// Đánh dấu token đã bị vô hiệu hóa (không verify nữa).
    /// </summary>
    public void Revoke()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("Cannot revoke a token that has already been used.");
        }
        UpdatedDate = DateTime.Now;
        IsRevoked = true;
    }
}
