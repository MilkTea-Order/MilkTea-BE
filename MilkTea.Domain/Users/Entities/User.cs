using MilkTea.Domain.Users.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Users.Entities;

/// <summary>
/// User entity (Aggregate Root).
/// </summary>
public sealed class User : Aggregate<int>
{
    public int EmployeesID { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Password { get; private set; } = null!;

    public UserStatus Status { get; private set; }

    public int? StoppedBy { get; private set; }
    public DateTime? StoppedDate { get; private set; }
    public int? PasswordResetBy { get; private set; }
    public DateTime? PasswordResetDate { get; private set; }


    public EmployeeProfile? Employee { get; private set; }
    private readonly List<RefreshToken> _vRefreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _vRefreshTokens.AsReadOnly();
    
    private readonly List<UserAndRole> _vUserRoles = new();
    public IReadOnlyList<UserAndRole> UserRoles => _vUserRoles.AsReadOnly();
    
    private readonly List<UserAndPermissionDetail> _vUserPermissions = new();
    public IReadOnlyList<UserAndPermissionDetail> UserPermissions => _vUserPermissions.AsReadOnly();

    // For EF Core
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
            EmployeesID = employeesId,
            UserName = userName,
            Password = passwordHash,
            Status = UserStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive => Status == UserStatus.Active;

    public void Deactivate(int stoppedBy)
    {
        if (Status == UserStatus.Inactive)
            throw new InvalidOperationException("User is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stoppedBy);

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

        if (Password == newPasswordHash)
            throw new InvalidOperationException("New password must be different from current password.");

        Password = newPasswordHash;
        PasswordResetBy = updatedBy;
        PasswordResetDate = DateTime.UtcNow;
        Touch(updatedBy);
    }

    public void ChangeUserName(string newUserName, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newUserName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        if (UserName == newUserName)
            throw new InvalidOperationException("New username must be different from current username.");

        UserName = newUserName;
        Touch(updatedBy);
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiryDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var refreshToken = RefreshToken.Create(Id, token, expiryDate);
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


    /// <summary>
    /// Gets all Role IDs that this user has.
    /// </summary>
    /// <returns>Collection of Role IDs assigned to this user.</returns>
    public IReadOnlyList<int> GetRoleIds()
    {
        return _vUserRoles
            .Select(ur => ur.RoleID)
            .Distinct()
            .ToList()
            .AsReadOnly();
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
