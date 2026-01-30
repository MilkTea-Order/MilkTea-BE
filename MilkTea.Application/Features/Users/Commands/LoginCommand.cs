using FluentValidation;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Commands;

public class LoginCommand : ICommand<LoginWithUserNameResult>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("Username");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("Password");
    }
}
