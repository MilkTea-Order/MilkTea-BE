namespace MilkTea.Domain.Users.Enums;

/// <summary>
/// Status of a user.
/// Maps to StatusID column in users table.
/// </summary>
public enum UserStatus
{
    Active = 1,     // Đang hoạt động
    Inactive = 2,   // Ngừng hoạt động
    Locked = 3      // Bị khóa
}
