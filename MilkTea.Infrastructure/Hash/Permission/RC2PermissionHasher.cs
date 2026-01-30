using MilkTea.Application.Ports.Hash.Permission;
using Shared.Utils;

namespace MilkTea.Infrastructure.Hash.Permission
{
    public class RC2PermissionHasher : IPermissionHasher
    {

        public const string key = "nguyentronghien0412@gmail.com";
        public const string iv = "0937410899@";
        public string DecodePermission(string hashedPermission)
        {
            var tmp = RC2Helper.EncryptByRC2(hashedPermission, key, iv);
            return RC2Helper.DecryptByRC2(hashedPermission, key, iv);
        }

        public string HashPermission(string permission)
        {
            return RC2Helper.EncryptByRC2(permission, key, iv);
        }

        public bool VerifyHashedPermission(string hashedPermission, string providedPermission)
        {
            return hashedPermission == RC2Helper.EncryptByRC2(providedPermission, key, iv);
        }
    }
}
