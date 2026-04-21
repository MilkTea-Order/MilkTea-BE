using MilkTea.Domain.Common.Exceptions;

namespace MilkTea.Domain.Auth.Exceptions
{
    public sealed class PasswordUsedException : DomainException
    {
        public PasswordUsedException() : base("Password has been used.") { }
    }

    public sealed class InvalidChannelException : DomainException
    {
        public InvalidChannelException() : base("Invalid channel.") { }
    }

    public sealed class InvalidFunctionSessionException : DomainException
    {
        public InvalidFunctionSessionException() : base("Invalid function session.") { }
    }

    public sealed class SessionExpiredException : DomainException
    {
        public SessionExpiredException() : base("Session has expired.") { }
    }

    public sealed class SessionAlreadyVerifiedException : DomainException
    {
        public SessionAlreadyVerifiedException() : base("Session has already been verified.") { }
    }

    public sealed class SessionNotFoundException : DomainException
    {
        public SessionNotFoundException() : base("Session not found.") { }
    }

    public sealed class OtpNotFoundException : DomainException
    {
        public OtpNotFoundException() : base("OTP not found or has expired.") { }
    }

    public sealed class InvalidOtpCodeException : DomainException
    {
        public InvalidOtpCodeException() : base("Invalid OTP code.") { }
    }
}
