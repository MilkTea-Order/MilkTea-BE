namespace MilkTea.Application.Models.Users
{
    /// <summary>
    /// Permission detail returned after login
    /// </summary>
    public sealed record UserPermission(
        int Id,
        int PermissionId,
        string? Note,
        PermissionInfo Permission
    );

    /// <summary>
    /// Permission basic info
    /// </summary>
    public sealed record PermissionInfo(
        int Id,
        string Name,
        string Code
    );
}
