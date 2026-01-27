using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Users.Commands;

public class LoginCommand : IRequest<LoginWithUserNameResult>
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
            .OverridePropertyName("Username")
            .WithMessage("UserName không được để trống");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("Password")
            .WithMessage("Password không được để trống");
    }
}
