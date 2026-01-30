namespace MilkTea.Application.Ports.Hash.Permission
{
    public interface IPermissionHasher
    {
        string HashPermission(string permission);
        bool VerifyHashedPermission(string hashedPermission, string providedPermission);

        string DecodePermission(string hashedPermission);
    }
}
