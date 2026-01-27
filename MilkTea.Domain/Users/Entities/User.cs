using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.Users.ValueObject;

namespace MilkTea.Domain.Users.Entities;

public sealed class User : Aggregate<int>
{
    private readonly List<RefreshToken> _vRefreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _vRefreshTokens.AsReadOnly();
    public int EmployeeID { get; private set; }

    public UserName UserName { get; private set; } = default!;
    public Password Password { get; private set; } = default!;

    public UserStatus Status { get; private set; }

    public int? StoppedBy { get; private set; }
    public DateTime? StoppedDate { get; private set; }

    public int? PasswordResetBy { get; private set; }
    public DateTime? PasswordResetDate { get; private set; }

    public bool IsActive => Status == UserStatus.Active;


    private User() { }

    public static User Create(
        int employeesId,
        string userName,
        string passwordHash,
        int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(employeesId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new User
        {
            EmployeeID = employeesId,
            UserName = UserName.Of(userName),
            Password = Password.Of(passwordHash),
            Status = UserStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }



    public void Deactivate(int stoppedBy)
    {
        if (Status == UserStatus.Inactive)
            throw new InvalidOperationException("User is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stoppedBy);

        // Revoke all active refresh tokens when deactivating user
        RevokeAllRefreshTokens(stoppedBy);

        Status = UserStatus.Inactive;
        StoppedBy = stoppedBy;
        StoppedDate = DateTime.UtcNow;
        Touch(stoppedBy);
    }

    public void Activate(int activatedBy)
    {
        if (Status == UserStatus.Active)
            throw new InvalidOperationException("User is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = UserStatus.Active;
        StoppedBy = null;
        StoppedDate = null;
        Touch(activatedBy);
    }

    public void UpdatePassword(string newPasswordHash, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newPasswordHash);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        var newPassword = Password.Of(newPasswordHash);
        if (Password.value == newPassword.value)
            throw new InvalidOperationException("New password must be different from current password.");

        Password = newPassword;
        PasswordResetBy = updatedBy;
        PasswordResetDate = DateTime.UtcNow;
        Touch(updatedBy);
    }

    public void ChangeUserName(string newUserName, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newUserName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        var newUserNameValue = UserName.Of(newUserName);
        if (UserName.value == newUserNameValue.value)
            throw new InvalidOperationException("New username must be different from current username.");

        UserName = newUserNameValue;
        Touch(updatedBy);
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiryDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var refreshToken = RefreshToken.Create(Id, token, expiryDate, CreatedBy);
        _vRefreshTokens.Add(refreshToken);
        return refreshToken;
    }

    public void RevokeRefreshToken(string token, int revokedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(revokedBy);

        var refreshToken = _vRefreshTokens.FirstOrDefault(rt => rt.Token == token && !rt.IsRevoked);
        if (refreshToken is null)
            throw new InvalidOperationException("Refresh token not found or already revoked.");

        refreshToken.Revoke(revokedBy);
    }

    public void RevokeAllRefreshTokens(int revokedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(revokedBy);

        foreach (var token in _vRefreshTokens.Where(rt => !rt.IsRevoked))
        {
            token.Revoke(revokedBy);
        }
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
