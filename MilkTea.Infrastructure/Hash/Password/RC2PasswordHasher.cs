using MilkTea.Application.Ports.Hash.Password;
using Shared.Utils;

namespace MilkTea.Infrastructure.Hash.Password
{
    public class RC2PasswordHasher : IPasswordHasher
    {
        private readonly static string key = "Password@!";
        private readonly static string iv = "0937410899@";
        public string HashPassword(string password)
        {
            return RC2Helper.EncryptByRC2(password, key, iv);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword == RC2Helper.EncryptByRC2(providedPassword, key, iv);
        }
    }
}
