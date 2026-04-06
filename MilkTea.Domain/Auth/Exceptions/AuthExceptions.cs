using MilkTea.Domain.Common.Exceptions;

namespace MilkTea.Domain.Auth.Exceptions
{
    public sealed class PasswordUsedException : DomainException
    {
        public PasswordUsedException() : base("Password has been used.") { }
    }


}
